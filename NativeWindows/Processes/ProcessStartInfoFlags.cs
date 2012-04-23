using System;

namespace NativeWindows.Processes
{
	[Flags]
	public enum ProcessStartInfoFlags
	{
		None = 0,
		UseShowWindow = 1,
		UseSize = 2,
		UsePosition = 4,
		UseCountChars = 8,
		UseFillAttribute = 0x10,
		RunFullscreen = 0x20,
		ForceOnFeedback = 0x40,
		ForceOffFeedback = 0x80,
		UseStdHandles = 0x100,
		UseHotKey = 0x200,
		TitleIsLinkName = 0x800,
		TitleIsAppId = 0x1000,
		PreventPinning = 0x2000,
	}
}
