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
