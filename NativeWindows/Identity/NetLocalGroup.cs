using System;
using System.Runtime.InteropServices;

namespace NativeWindows.Identity
{
	public class NetLocalGroup : INetLocalGroup
	{
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct LocalGroupInfo1
		{
			public string GroupName;
			public string Comment;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct LocalGroupMembersInfo3
		{
			public string DomainAndUsername;
		}

		private class NativeMethods
		{
			[DllImport("netapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern SystemErrorCode NetLocalGroupAdd(
				[MarshalAs(UnmanagedType.LPWStr)]string serverName,
				uint level,
				ref LocalGroupInfo1 groupInfo,
				out uint parameterErrorIndex);

			[DllImport("netapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern SystemErrorCode NetLocalGroupAddMembers(
				[MarshalAs(UnmanagedType.LPWStr)]string serverName,
				[MarshalAs(UnmanagedType.LPWStr)]string groupName,
				uint level,
				ref LocalGroupMembersInfo3 groupMemberInfo,
				uint totalEntries);

			[DllImport("netapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern SystemErrorCode NetLocalGroupGetInfo(
				[MarshalAs(UnmanagedType.LPWStr)]string serverName,
				[MarshalAs(UnmanagedType.LPWStr)]string groupName,
				uint level,
				out IntPtr groupInfo);

			[DllImport("netapi32.dll", SetLastError = true)]
			public static extern SystemErrorCode NetApiBufferFree(IntPtr buffer);
		}

		public void Add(string groupName, string description, string serverName = null)
		{
			var groupInfo = new LocalGroupInfo1
			{
				GroupName = groupName,
				Comment = description,
			};

			uint parameterErrorIndex;
			var result = NativeMethods.NetLocalGroupAdd(null, 1, ref groupInfo, out parameterErrorIndex);
			if (result != SystemErrorCode.ErrorSuccess)
			{
				throw ErrorHelper.GetWin32Exception(result);
			}
		}

		public void AddMember(string username, string groupName, string domainName = null, string serverName = null)
		{
			if (domainName == null)
			{
				domainName = Environment.MachineName;
			}

			var membersInfo3 = new LocalGroupMembersInfo3
			{
				DomainAndUsername = string.Format("{0}\\{1}", domainName, username),
			};

			var result = NativeMethods.NetLocalGroupAddMembers(serverName, groupName, 3, ref membersInfo3, 1);
			if (result != SystemErrorCode.ErrorSuccess)
			{
				throw ErrorHelper.GetWin32Exception(result);
			}
		}

		public bool Exists(string groupName, string serverName = null)
		{
			IntPtr groupInfoPtr;
			var result = NativeMethods.NetLocalGroupGetInfo(null, groupName, 1, out groupInfoPtr);
			try
			{
				switch (result)
				{
					case SystemErrorCode.ErrorSuccess:
						return true;
					case SystemErrorCode.NetErrorGroupNotFound:
						return false;
					default:
						throw ErrorHelper.GetWin32Exception(result);
				}
			}
			finally
			{
				Free(groupInfoPtr);
			}
		}

		private void Free(IntPtr groupInfoPtr)
		{
			if (groupInfoPtr != IntPtr.Zero)
			{
				var result = NativeMethods.NetApiBufferFree(groupInfoPtr);
				if (result != SystemErrorCode.ErrorSuccess)
				{
					throw ErrorHelper.GetWin32Exception(result);
				}
			}
		}
	}
}
