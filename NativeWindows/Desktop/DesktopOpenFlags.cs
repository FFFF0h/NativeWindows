using System;

namespace NativeWindows.Desktop
{
	[Flags]
	public enum DesktopOpenFlags : uint
	{
		None = 0x00000000,
		AllowOtherAccountHook = 0x00000001,
	}
}
