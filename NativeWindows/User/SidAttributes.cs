using System;

namespace NativeWindows.User
{
	[Flags]
	public enum SidAttributes : uint
	{
		SeGroupMandatory = 0x00000001,
		SeGroupEnabledByDefault = 0x00000002,
		SeGroupEnabled = 0x00000004,
		SeGroupOwner = 0x00000008,
		SeGroupUseForDenyOnly = 0x00000010,
		SeGroupIntegrity = 0x00000020,
		SeGroupIntegrityEnabled = 0x00000040,
		SeGroupResource = 0x20000000,
		SeGroupLogonId = 0xC0000000,
	}
}
