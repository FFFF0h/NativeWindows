using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using NativeWindows.ErrorHandling;
using NativeWindows.User;

namespace NativeWindows.ProcessAndThread
{
	/// <remarks>
	/// Process is completly inconsistent, -1 is a valid handle and means current Process (most functions allows for this handle to be used)
	/// </remarks>>
	public sealed class ProcessHandle : SafeHandleZeroIsInvalid
	{
		private readonly Lazy<ProcessExitMonitor> _exitMonitor;

		[StructLayout(LayoutKind.Sequential)]
		private struct ProcessInformationOut
		{
			public IntPtr ProcessHandle;
			public IntPtr ThreadHandle;
			public int ProcessId;
			public int ThreadId;
		}

		private class ProcessWaitHandle : WaitHandle
		{
			public ProcessWaitHandle(ProcessHandle processHandle)
			{
				SafeWaitHandle waitHandle;
				if (!NativeMethods.DuplicateHandle(GetCurrentProcess(), processHandle, GetCurrentProcess(), out waitHandle, 0, false, DuplicateHandleOptions.SameAccess))
				{
					ErrorHelper.ThrowCustomWin32Exception();
				}
				SafeWaitHandle = waitHandle;
			}
		}

		private class ProcessExitMonitor
		{
			private readonly ProcessWaitHandle _processWaitHandle;
			private readonly TaskCompletionSource<int> _completion = new TaskCompletionSource<int>();

			public ProcessExitMonitor(ProcessHandle processHandle)
			{
				_processWaitHandle = new ProcessWaitHandle(processHandle);
				ThreadPool.RegisterWaitForSingleObject(_processWaitHandle, HandleExited, null, -1, true);
			}

			private void HandleExited(object state, bool timedout)
			{
				int exitCode;
				if (!NativeMethods.GetExitCodeProcess(_processWaitHandle.SafeWaitHandle, out exitCode))
				{
					_completion.SetException(new Win32Exception());
				}
				_processWaitHandle.Dispose();
				_completion.SetResult(exitCode);
			}

			public Task<int> Task
			{
				get
				{
					return _completion.Task;
				}
			}
		}

		private static class NativeMethods
		{
			[DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
			public static extern bool CreateProcessAsUser(UserHandle userHandle, string applicationName, string commandLine, SecurityAttributes processAttributes, SecurityAttributes threadAttributes, bool inheritHandles, ProcessCreationFlags creationFlags, EnvironmentBlockHandle environment, string currentDirectory, ProcessStartInfo startInfo, out ProcessInformationOut processInformation);

			[DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
			public static extern bool CreateProcessWithLogonW(string username, string domain, string password, ProcessLogonFlags logonFlags, string applicationName, string commandLine, ProcessCreationFlags creationFlags, EnvironmentBlockHandle environment, string currentDirectory, ProcessStartInfo startupInfo, out ProcessInformationOut processInformation);

			[DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
			[ResourceExposure(ResourceScope.Process)]
			public static extern ProcessHandle GetCurrentProcess();

			[DllImport("kernel32.dll", SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool GetExitCodeProcess(SafeWaitHandle processHandle, out int exitCode);

			[DllImport("kernel32.dll", SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool GetExitCodeProcess(ProcessHandle processHandle, out int exitCode);

			[DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true, BestFitMapping = false)]
			[ResourceExposure(ResourceScope.Machine)]
			public static extern bool DuplicateHandle(ProcessHandle sourceProcessHandle, ProcessHandle sourceHandle, ProcessHandle targetProcess, out SafeWaitHandle targetHandle, uint desiredAccess, bool inheritHandle, DuplicateHandleOptions options);

			[DllImport("kernel32.dll", SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool TerminateProcess(ProcessHandle processHandle, int exitCode);

			[DllImport("kernel32.dll", SetLastError = true)]
			public static extern int GetProcessId(ProcessHandle processHandle);

			[DllImport("kernel32.dll", SetLastError = true)]
			public static extern ProcessHandle OpenProcess(ProcessAccessRights desiredAccess, bool inheritHandle, int processId);
		}

		public static ProcessInformation CreateAsUser(UserHandle userHandle, string applicationName, string commandLine, bool inheritHandles, ProcessCreationFlags creationFlags, EnvironmentBlockHandle environmentHandle, string currentDirectory, ProcessStartInfo startInfo)
		{
			using (var processSecurityAttributes = new SecurityAttributes())
			{
				using (var threadSecurityAttributes = new SecurityAttributes())
				{
					ProcessInformationOut processInformation;
					if (!NativeMethods.CreateProcessAsUser(userHandle, applicationName, commandLine, processSecurityAttributes, threadSecurityAttributes, inheritHandles, creationFlags, environmentHandle, currentDirectory, startInfo, out processInformation))
					{
						ErrorHelper.ThrowCustomWin32Exception();
					}
					return new ProcessInformation(processInformation.ProcessHandle, processInformation.ProcessId, processInformation.ThreadHandle, processInformation.ThreadId);
				}
			}
		}

		public static ProcessInformation CreateWithLogin(string username, string domain, string password, ProcessLogonFlags logonFlags, string applicationName, string commandLine, ProcessCreationFlags creationFlags, EnvironmentBlockHandle environment, string currentDirectory, ProcessStartInfo startupInfo)
		{
			ProcessInformationOut processInformation;
			if (!NativeMethods.CreateProcessWithLogonW(username, domain, password, logonFlags, applicationName, commandLine, creationFlags, environment, currentDirectory, startupInfo, out processInformation))
			{
				throw new Win32Exception();
			}
			return new ProcessInformation(processInformation.ProcessHandle, processInformation.ProcessId, processInformation.ThreadHandle, processInformation.ThreadId);
		}

		public static ProcessHandle OpenProcess(int processId, ProcessAccessRights desiredAccess = ProcessAccessRights.AllAccess, bool inheritHandle = false)
		{
			ProcessHandle handle = NativeMethods.OpenProcess(desiredAccess, inheritHandle, processId);
			if (handle.IsInvalid)
			{
				ErrorHelper.ThrowCustomWin32Exception();
			}
			return handle;
		}

		public static ProcessHandle GetCurrentProcess()
		{
			return NativeMethods.GetCurrentProcess();
		}

		public ProcessHandle()
			: base(true)
		{
			_exitMonitor = new Lazy<ProcessExitMonitor>(() => new ProcessExitMonitor(this));
		}

		public ProcessHandle(IntPtr handle, bool ownsHandle = true)
			: base(ownsHandle)
		{
			_exitMonitor = new Lazy<ProcessExitMonitor>(() => new ProcessExitMonitor(this));
			SetHandle(handle);
		}

		public bool HasExited
		{
			get
			{
				return WaitForExit(TimeSpan.Zero);
			}
		}

		public int GetProcessId()
		{
			return NativeMethods.GetProcessId(this);
		}

		public void Terminate(int exitCode)
		{
			if (!NativeMethods.TerminateProcess(this, exitCode))
			{
				ErrorHelper.ThrowCustomWin32Exception();
			}
		}

		public Task<int> Completion
		{
			get
			{
				return _exitMonitor.Value.Task;
			}
		}

		public int GetExitCode()
		{
			int exitCode;
			if (!NativeMethods.GetExitCodeProcess(this, out exitCode))
			{
				ErrorHelper.ThrowCustomWin32Exception();
			}
			return exitCode;
		}

		public bool WaitForExit(TimeSpan timeout)
		{
			if (IsInvalid || IsClosed)
			{
				return true;
			}
			using (var waitHandle = new ProcessWaitHandle(this))
			{
				return waitHandle.WaitOne(timeout);
			}
		}

		protected override bool ReleaseHandle()
		{
			if (handle != new IntPtr(-1))
			{
				return handle.CloseHandle();
			}
			return true;
		}
	}
}
