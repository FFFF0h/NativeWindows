using System;

namespace NativeWindows
{
	[Flags]
	public enum DuplicateHandleOptions
	{
		CloseSource = 0x00000001,
		SameAccess = 0x00000002,
	}
}
