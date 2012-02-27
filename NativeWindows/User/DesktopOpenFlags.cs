using System;

namespace NativeWindows.User
{
	[Flags]
	public enum DesktopOpenFlags : uint
	{
		None = 0x00000000,
		AllowOtherAccountHook = 0x00000001,
	}
}
