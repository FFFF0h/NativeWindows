using System;

namespace NativeWindows.ProcessAndThread
{
	[Flags]
	public enum DuplicateHandleOptions
	{
		CloseSource = 0x00000001,
		SameAccess = 0x00000002,
	}
}
