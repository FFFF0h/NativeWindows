using System;

namespace NativeWindows.User
{
	[Flags]
	public enum DesktopAccessRights : uint
	{
		Delete = 0x00010000,
		ReadControl = 0x00020000,
		WriteDac = 0x00040000,
		WriteOwner = 0x00080000,
		Synchronize = 0x00100000,

		ReadObjects = 0x00000001,
		Createwindow = 0x00000002,
		CreateMenu = 0x00000004,
		HookControl = 0x00000008,
		JournalRecord = 0x00000010,
		JournalPlayback = 0x00000020,
		Enumerate = 0x00000040,
		Writeobjects = 0x00000080,
		SwitchDesktop = 0x00000100,
		ReadScreen = 0x00000200,
	}
}
