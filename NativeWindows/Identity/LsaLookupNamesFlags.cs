using System;

namespace NativeWindows.Identity
{
	[Flags]
	public enum LsaLookupNamesFlags : uint
	{
		None = 0,
		LsaLookupIsolatedAsLocal = 0x80000000,
	}
}
