using System;
using System.Runtime.InteropServices;
using System.Security;
using NativeWindows.ErrorHandling;

namespace NativeWindows.User
{
	/// <remarks>
	/// This is also known as a user token in some of the native methods
	/// </remarks>>
	public sealed class UserHandle : SafeHandle
	{
		private static class NativeMethods
		{
			[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern bool LogonUser(string username, string domain, IntPtr password, UserLogonType logonType, UserLogonProvider logonProvider, out UserHandle handle);

			[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern bool LogonUser(string username, string domain, string password, UserLogonType logonType, UserLogonProvider logonProvider, out UserHandle handle);

			[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			public static extern bool DuplicateTokenEx(UserHandle handle, TokenAccessRights desiredAccess, SecurityAttributes securityAttributes, SecurityImpersonationLevel impersonationLevel, TokenType tokenType, out UserHandle newToken);
		}

		public static UserHandle Logon(string username, string domain, SecureString password, UserLogonType logonType = UserLogonType.Interactive, UserLogonProvider logonProvider = UserLogonProvider.Default)
		{
			IntPtr passwordPtr = Marshal.SecureStringToGlobalAllocUnicode(password);
			try
			{
				UserHandle userHandle;
				if (!NativeMethods.LogonUser(username, domain, passwordPtr, logonType, logonProvider, out userHandle))
				{
					ErrorHelper.ThrowCustomWin32Exception();
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
				ErrorHelper.ThrowCustomWin32Exception();
			}
			return userHandle;
		}

		public UserHandle()
			: base(IntPtr.Zero, true)
		{
		}

		public UserHandle DuplicateTokenEx(TokenAccessRights desiredAccess, SecurityImpersonationLevel impersonationLevel, TokenType tokenType)
		{
			UserHandle newHandle;
			if (!NativeMethods.DuplicateTokenEx(this, desiredAccess, null, impersonationLevel, tokenType, out newHandle))
			{
				ErrorHelper.ThrowCustomWin32Exception();
			}
			return newHandle;
		}

		protected override bool ReleaseHandle()
		{
			return handle.CloseHandle();
		}

		public override bool IsInvalid
		{
			get
			{
				return handle == IntPtr.Zero || handle == new IntPtr(-1);
			}
		}
	}
}
