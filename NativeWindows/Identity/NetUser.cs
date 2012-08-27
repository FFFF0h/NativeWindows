using System;
using System.Runtime.InteropServices;

namespace NativeWindows.Identity
{
	public class NetUser : INetUser
	{
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct UserInfo1
		{
			public UserInfo1(NetUserInformation userInformation)
			{
				Name = userInformation.Username;
				Password = userInformation.Password;
				PasswordAge = userInformation.PasswordAge;
				Privilege = userInformation.Privilege;
				HomeDirectory = userInformation.HomeDirectory;
				Comment = userInformation.Comment;
				Flags = userInformation.Flags;
				ScriptPath = userInformation.ScriptPath;
			}

			public string Name;
			public string Password;
			public uint PasswordAge;
			public NetUserPrivilige Privilege;
			public string HomeDirectory;
			public string Comment;
			public NetUserFlags Flags;
			public string ScriptPath;
		}

		private class NativeMethods
		{
			[DllImport("netapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern SystemErrorCode NetUserAdd(
				[MarshalAs(UnmanagedType.LPWStr)]string serverName,
				uint level,
				ref UserInfo1 userInfo,
				out uint parameterErrorIndex);

			[DllImport("netapi32.dll")]
			public static extern SystemErrorCode NetUserDel(
				[MarshalAs(UnmanagedType.LPWStr)]string serverName,
				[MarshalAs(UnmanagedType.LPWStr)]string username);

			[DllImport("netapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern SystemErrorCode NetUserGetInfo(
				[MarshalAs(UnmanagedType.LPWStr)]string serverName,
				[MarshalAs(UnmanagedType.LPWStr)]string username,
				uint level,
				out IntPtr userInfo);

			[DllImport("netapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern SystemErrorCode NetUserSetInfo(
				[MarshalAs(UnmanagedType.LPWStr)]string serverName,
				[MarshalAs(UnmanagedType.LPWStr)]string username,
				uint level,
				ref UserInfo1 userInfo,
				out uint parameterErrorIndex);

			[DllImport("netapi32.dll", SetLastError = true)]
			public static extern SystemErrorCode NetApiBufferFree(IntPtr buffer);
		}

		public void Add(NetUserInformation userInformation, string serverName = null)
		{
			var userInfo1 = new UserInfo1(userInformation);

			uint parameterErrorIndex;
			var result = NativeMethods.NetUserAdd(serverName, 1, ref userInfo1, out parameterErrorIndex);
			if (result != SystemErrorCode.ErrorSuccess)
			{
				throw ErrorHelper.GetWin32Exception(result);
			}
		}

		public void Delete(string username, string serverName = null)
		{
			var result = NativeMethods.NetUserDel(serverName, username);
			if (result != SystemErrorCode.ErrorSuccess && result != SystemErrorCode.NetErrorUserNotFound)
			{
				throw ErrorHelper.GetWin32Exception(result);
			}
		}

		public bool Exists(string username, string serverName = null)
		{
			IntPtr userInfoPtr;
			var result = NativeMethods.NetUserGetInfo(null, username, 1, out userInfoPtr);
			try
			{
				switch (result)
				{
					case SystemErrorCode.ErrorSuccess:
						return true;
					case SystemErrorCode.NetErrorUserNotFound:
						return false;
					default:
						throw ErrorHelper.GetWin32Exception(result);
				}
			}
			finally
			{
				Free(userInfoPtr);
			}
		}

		public NetUserInformation GetInformation(string username, string serverName = null)
		{
			IntPtr userInfoPtr;
			var result = NativeMethods.NetUserGetInfo(serverName, username, 1, out userInfoPtr);
			try
			{
				if (result != SystemErrorCode.ErrorSuccess)
				{
					throw ErrorHelper.GetWin32Exception(result);
				}
				var userInfoStructure = (UserInfo1)Marshal.PtrToStructure(userInfoPtr, typeof(UserInfo1));
				return new NetUserInformation
				{
					Username = userInfoStructure.Name,
					Password = userInfoStructure.Password,
					PasswordAge = userInfoStructure.PasswordAge,
					Privilege = userInfoStructure.Privilege,
					HomeDirectory = userInfoStructure.HomeDirectory,
					Comment = userInfoStructure.Comment,
					Flags = userInfoStructure.Flags,
					ScriptPath = userInfoStructure.ScriptPath,
				};
			}
			finally
			{
				Free(userInfoPtr);
			}
		}

		public void SetInformation(NetUserInformation information, string serverName = null)
		{
			var userInfo1 = new UserInfo1(information);
			uint parameterErrorIndex;
			var result = NativeMethods.NetUserSetInfo(serverName, information.Username, 1, ref userInfo1, out parameterErrorIndex);
			if (result != SystemErrorCode.ErrorSuccess)
			{
				throw ErrorHelper.GetWin32Exception(result);
			}
		}

		private void Free(IntPtr userInfoPtr)
		{
			if (userInfoPtr != IntPtr.Zero)
			{
				var result = NativeMethods.NetApiBufferFree(userInfoPtr);
				if (result != SystemErrorCode.ErrorSuccess)
				{
					throw ErrorHelper.GetWin32Exception(result);
				}
			}
		}
	}
}
