using System;

namespace NativeWindows.User
{
	[Flags]
	public enum WindowStationAccessRights
	{
		None = 0,

		EnumDesktops = 1,
		ReadAttributes = 2,
		AccessClipboard = 4,
		CreateDesktop = 8,
		WriteAttributes = 16,
		AccessGlobalAtoms = 32,
		ExitWindows = 64,
		Enumerate = 256,
		ReadScreen = 512,

		Delete = 0x00010000,
		ReadPermissions = 0x00020000,
		WritePermissions = 0x00040000,
		TakeOwnership = 0x00080000,
		Synchronize = 0x00100000,

		Read = StandardAccessRights.Read | EnumDesktops | ReadAttributes | Enumerate | ReadScreen,
		Write = StandardAccessRights.Write | AccessClipboard | CreateDesktop | WriteAttributes,
		Execute = StandardAccessRights.Execute | AccessGlobalAtoms | ExitWindows,

		AllAccess = StandardAccessRights.Required | EnumDesktops | ReadAttributes | AccessClipboard | CreateDesktop | WriteAttributes | AccessGlobalAtoms | ExitWindows | Enumerate | ReadScreen,
	}
}
