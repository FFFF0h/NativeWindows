using System;

namespace NativeWindows.ProcessAndThread
{
	[Flags]
	public enum DuplicateHandleOptions
	{
		DuplicateCloseSource = 0x00000001,
		DuplicateSameAccess = 0x00000002,
	}
}
