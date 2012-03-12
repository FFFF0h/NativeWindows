using System;

namespace NativeWindows.User
{
	[Flags]
	public enum DesktopAccessRights : uint
	{
		None = 0,

		ReadObjects = 1,
		CreateWindow = 2,
		CreateMenu = 4,
		HookControl = 8,
		JournalRecord = 16,
		JournalPlayback = 32,
		Enumerate = 64,
		WriteObjects = 128,
		SwitchDesktop = 256,

		Delete = 0x00010000,
		ReadPermissions = 0x00020000,
		WritePermissions = 0x00040000,
		TakeOwnership = 0x00080000,
		Synchronize = 0x00100000,

		Read = StandardAccessRights.Read | ReadObjects | Enumerate,
		Write = StandardAccessRights.Write | WriteObjects | CreateWindow | CreateMenu | HookControl | JournalRecord | JournalPlayback,
		Execute = StandardAccessRights.Execute | SwitchDesktop,

		AllAccess = StandardAccessRights.Required | ReadObjects | CreateWindow | CreateMenu | HookControl | JournalRecord | JournalPlayback | Enumerate | WriteObjects | SwitchDesktop,
	}
}
