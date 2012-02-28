using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using NativeWindows.User;

namespace NativeWindows.ProcessAndThread
{
	/// <remarks>
	/// Process is completly inconsistent, -1 is a valid handle and means current Process (most functions allows for this handle to be used)
	/// </remarks>>
	public sealed class ProcessHandle : SafeHandleZeroIsInvalid
	{
		private static class NativeMethods
		{
			[DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
			public static extern bool CreateProcessAsUser(UserHandle userHandle, string applicationName, string commandLine, SecurityAttributes processAttributes, SecurityAttributes threadAttributes, bool inheritHandles, ProcessCreationFlags creationFlags, EnvironmentBlockHandle environment, string currentDirectory, ProcessStartInfo startInfo, out ProcessInformation processInformation);

			[DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
			[ResourceExposure(ResourceScope.Process)]
			public static extern ProcessHandle GetCurrentProcess();

			[DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true, BestFitMapping = false)]
			[ResourceExposure(ResourceScope.Machine)]
			public static extern bool DuplicateHandle<T>(ProcessHandle sourceProcessHandle, SafeHandle sourceHandle, ProcessHandle targetProcess, out T targetHandle, uint desiredAccess, bool inheritHandle, DuplicateHandleOptions options) where T : SafeHandle;
		}

		public static ProcessInformation CreateAsUser(UserHandle userHandle, string applicationName, string commandLine, bool inheritHandles, ProcessCreationFlags creationFlags, EnvironmentBlockHandle environmentHandle, string currentDirectory, ProcessStartInfo startInfo)
		{
			using (var processSecurityAttributes = new SecurityAttributes())
			{
				using (var threadSecurityAttributes = new SecurityAttributes())
				{
					ProcessInformation processInformation;
					if (!NativeMethods.CreateProcessAsUser(userHandle, applicationName, commandLine, processSecurityAttributes, threadSecurityAttributes, inheritHandles, creationFlags, environmentHandle, currentDirectory, startInfo, out processInformation))
					{
						throw new Win32Exception();
					}
					return processInformation;
				}
			}
		}

		public static ProcessHandle GetCurrentProcess()
		{
			return NativeMethods.GetCurrentProcess();
		}

		public T DuplicateHandle<T>(T sourceHandle, ProcessHandle targetProcess, uint desiredAccess, bool inheritHandle, DuplicateHandleOptions options) where T : SafeHandle
		{
			T targetHandle;
			if (!NativeMethods.DuplicateHandle(this, sourceHandle, targetProcess, out targetHandle, desiredAccess, inheritHandle, options))
			{
				throw new Win32Exception();
			}
			return targetHandle;
		}

		public T DuplicateHandle<T>(T sourceHandle, uint desiredAccess, bool inheritHandle, DuplicateHandleOptions options) where T : SafeHandle
		{
			T targetHandle;
			if (!NativeMethods.DuplicateHandle(this, sourceHandle, this, out targetHandle, desiredAccess, inheritHandle, options))
			{
				throw new Win32Exception();
			}
			return targetHandle;
		}

		public ProcessHandle()
			: base(true)
		{
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
