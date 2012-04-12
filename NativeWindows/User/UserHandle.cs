﻿using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Principal;
using NativeWindows.ErrorHandling;
using NativeWindows.ProcessAndThread;

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

			[DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true, BestFitMapping = false)]
			[ResourceExposure(ResourceScope.Machine)]
			public static extern bool DuplicateHandle(ProcessHandle sourceProcessHandle, IntPtr sourceHandle, ProcessHandle targetProcess, out IntPtr targetHandle, uint desiredAccess, bool inheritHandle, DuplicateHandleOptions options);
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

		private static IntPtr DuplicateHandle(IntPtr tokenHandle)
		{
			IntPtr duplicated;
			if (!NativeMethods.DuplicateHandle(ProcessHandle.GetCurrentProcess(), tokenHandle, ProcessHandle.GetCurrentProcess(), out duplicated, 0, false, DuplicateHandleOptions.SameAccess))
			{
				ErrorHelper.ThrowCustomWin32Exception();
			}
			return duplicated;
		}

		public UserHandle()
			: base(IntPtr.Zero, true)
		{
		}

		public UserHandle(WindowsIdentity identity)
			: base(DuplicateHandle(identity.Token), true)
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
