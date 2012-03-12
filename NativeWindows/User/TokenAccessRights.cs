using System;

namespace NativeWindows.User
{
	// http://msdn.microsoft.com/en-us/library/windows/desktop/aa374905(v=vs.85).aspx
	[Flags]
	public enum TokenAccessRights
	{
		None = 0,

		AssignPrimary = 1,
		Duplicate = 2,
		Impersonate = 4,
		Query = 8,
		QuerySource = 16,
		AdjustPrivileges = 32,
		AdjustGroups = 64,
		AdjustDefault = 128,
		AdjustSessionId = 256,

		Delete = 0x00010000,
		ReadPermissions = 0x00020000,
		WritePermissions = 0x00040000,
		TakeOwnership = 0x00080000,
		Synchronize = 0x00100000,

		Read = StandardAccessRights.Read | Query,
		Write = StandardAccessRights.Write | AdjustPrivileges | AdjustGroups | AdjustDefault,
		Execute = StandardAccessRights.Execute,

		AllAccess = StandardAccessRights.Required | AssignPrimary | Duplicate | Impersonate | Query | QuerySource | AdjustPrivileges | AdjustGroups | AdjustDefault | AdjustSessionId,
	}
}
