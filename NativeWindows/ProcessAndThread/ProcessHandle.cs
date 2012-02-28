using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using Microsoft.Win32.SafeHandles;
using NativeWindows.User;

namespace NativeWindows.ProcessAndThread
{
	/// <remarks>
	/// Process is completly inconsistent, -1 is a valid handle and means current Process (most functions allows for this handle to be used)
	/// </remarks>>
	public sealed class ProcessHandle : SafeHandleZeroIsInvalid
	{
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
				if (!NativeMethods.DuplicateHandle(GetCurrentProcess(), processHandle, GetCurrentProcess(), out waitHandle, 0, false, DuplicateHandleOptions.DuplicateSameAccess))
				{
					throw new Win32Exception();
				}
				SafeWaitHandle = waitHandle;
			}
		}

		private static class NativeMethods
		{
			[DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
			public static extern bool CreateProcessAsUser(UserHandle userHandle, string applicationName, string commandLine, SecurityAttributes processAttributes, SecurityAttributes threadAttributes, bool inheritHandles, ProcessCreationFlags creationFlags, EnvironmentBlockHandle environment, string currentDirectory, ProcessStartInfo startInfo, out ProcessInformationOut processInformation);

			[DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
			[ResourceExposure(ResourceScope.Process)]
			public static extern ProcessHandle GetCurrentProcess();

			[DllImport("kernel32.dll", SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool GetExitCodeProcess(ProcessHandle processHandle, out int exitCode);

			[DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true, BestFitMapping = false)]
			[ResourceExposure(ResourceScope.Machine)]
			public static extern bool DuplicateHandle(ProcessHandle sourceProcessHandle, ProcessHandle sourceHandle, ProcessHandle targetProcess, out SafeWaitHandle targetHandle, uint desiredAccess, bool inheritHandle, DuplicateHandleOptions options);
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
						throw new Win32Exception();
					}
					return new ProcessInformation(processInformation.ProcessHandle, processInformation.ProcessId, processInformation.ThreadHandle, processInformation.ThreadId);
				}
			}
		}

		public static ProcessHandle GetCurrentProcess()
		{
			return NativeMethods.GetCurrentProcess();
		}

		public ProcessHandle()
			: base(true)
		{
		}

		public ProcessHandle(IntPtr handle, bool ownsHandle = true)
			: base(ownsHandle)
		{
			SetHandle(handle);
		}

		public int GetExitCode()
		{
			int exitCode;
			if (!NativeMethods.GetExitCodeProcess(this, out exitCode))
			{
				throw new Win32Exception();
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
