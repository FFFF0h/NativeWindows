using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace NativeWindows.User
{
	/// <remarks>
	/// This is also known as a user token in some of the native methods
	/// </remarks>>
	public sealed class UserHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		private static class NativeMethods
		{
			[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern bool LogonUser(string username, string domain, IntPtr password, UserLogonType logonType, UserLogonProvider logonProvider, out UserHandle handle);

			[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern bool LogonUser(string username, string domain, string password, UserLogonType logonType, UserLogonProvider logonProvider, out UserHandle handle);
		}

		public static UserHandle Logon(string username, string domain, SecureString password, UserLogonType logonType = UserLogonType.Interactive, UserLogonProvider logonProvider = UserLogonProvider.Default)
		{
			IntPtr passwordPtr = Marshal.SecureStringToGlobalAllocUnicode(password);
			try
			{
				UserHandle userHandle;
				if (!NativeMethods.LogonUser(username, domain, passwordPtr, logonType, logonProvider, out userHandle))
				{
					throw new Win32Exception();
				}
				return userHandle;
			}
			finally
			{
				Marshal.ZeroFreeGlobalAllocUnicode(passwordPtr);
			}
		}

		public static UserHandle Logon(string username, string domain, string password, UserLogonType logonType = UserLogonType.Interactive, UserLogonProvider logonProvider = UserLogonProvider.Default)
		{
			UserHandle userHandle;
			if (!NativeMethods.LogonUser(username, domain, password, logonType, logonProvider, out userHandle))
			{
				throw new Win32Exception();
			}
			return userHandle;
		}

		public UserHandle() : base(true)
		{
		}

		protected override bool ReleaseHandle()
		{
			return handle.CloseHandle();
		}
	}
}
