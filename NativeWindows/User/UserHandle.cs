using System;
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

			[DllImport("advapi32.dll", SetLastError = true)]
			public static extern bool GetTokenInformation(IntPtr tokenHandle, TokenInformationClass tokenInformationClass, IntPtr tokenInformation, int tokenInformationLength, out int returnLength);

			[DllImport("userenv.dll", CharSet = CharSet.Auto, SetLastError = true)]
			public static extern bool LoadUserProfile(UserHandle token, ref ProfileInfo profileInfo);

			[DllImport("Userenv.dll", CharSet = CharSet.Auto, SetLastError = true)]
			public static extern bool UnloadUserProfile(UserHandle token, IntPtr profileInfo);
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct NativeTokenGroups
		{
			public int GroupCount;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
			public NativeSidAndAttributes[] Groups;
		};

		[StructLayout(LayoutKind.Sequential)]
		private struct NativeSidAndAttributes
		{
			public IntPtr Sid;
			public SidAttributes Attributes;
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

		public void LoadUserProfile(ref ProfileInfo profileInfo)
		{
			if (!NativeMethods.LoadUserProfile(this, ref profileInfo))
			{
				ErrorHelper.ThrowCustomWin32Exception();
			}
		}

		public void UnloadUserProfile(ref ProfileInfo profileInfo)
		{
			if (!NativeMethods.UnloadUserProfile(this, profileInfo.Profile))
			{
				ErrorHelper.ThrowCustomWin32Exception();
			}
		}

		public unsafe SidAndAttributes[] GetGroupsTokenInformation(TokenInformationClass tokenInformationClass)
		{
			if (tokenInformationClass != TokenInformationClass.TokenGroups &&
				tokenInformationClass != TokenInformationClass.TokenRestrictedSids &&
				tokenInformationClass != TokenInformationClass.TokenLogonSid &&
				tokenInformationClass != TokenInformationClass.TokenCapabilities &&
				tokenInformationClass != TokenInformationClass.TokenDeviceGroups &&
				tokenInformationClass != TokenInformationClass.TokenRestrictedDeviceGroups)
			{
				throw new ArgumentException(string.Format("{0} is not a valid Group token information class", tokenInformationClass), "tokenInformationClass");
			}

			int tokenSize;
			if (!NativeMethods.GetTokenInformation(handle, tokenInformationClass, IntPtr.Zero, 0, out tokenSize))
			{
				if (Marshal.GetLastWin32Error() != (int)SystemErrorCode.ErrorInsufficientBuffer)
				{
					ErrorHelper.ThrowCustomWin32Exception();
				}
			}

			IntPtr tokenInformationPtr = Marshal.AllocHGlobal(tokenSize);
			try
			{
				if (!NativeMethods.GetTokenInformation(handle, tokenInformationClass, tokenInformationPtr, tokenSize, out tokenSize))
				{
					ErrorHelper.ThrowCustomWin32Exception();
				}
				var groupStructure = (NativeTokenGroups)Marshal.PtrToStructure(tokenInformationPtr, typeof(NativeTokenGroups));
				IntPtr offset = Marshal.OffsetOf(typeof(NativeTokenGroups), "Groups");
				var groups = new SidAndAttributes[groupStructure.GroupCount];
				long start = new IntPtr(tokenInformationPtr.ToInt64() + offset.ToInt64()).ToInt64();
				for (int i = 0; i < groupStructure.GroupCount; i++)
				{
					var arrayElementPtr = new IntPtr(start + i * sizeof(NativeSidAndAttributes));
					var group = (NativeSidAndAttributes)Marshal.PtrToStructure(arrayElementPtr, typeof(NativeSidAndAttributes));
					groups[i] = new SidAndAttributes(group.Attributes, group.Sid);
				}
				return groups;
			}
			finally
			{
				Marshal.FreeHGlobal(tokenInformationPtr);
			}
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
