using System;

namespace NativeWindows.Security
{
	[Flags]
	public enum SecurityInformation : uint
	{
		OwnerSecurityInformation = 0x00000001,
		GroupSecurityInformation = 0x00000002,
		DaclSecurityInformation = 0x00000004,
		SaclSecurityInformation = 0x00000008,
		UnprotectedSaclSecurityInformation = 0x10000000,
		UnprotectedDaclSecurityInformation = 0x20000000,
		ProtectedSaclSecurityInformation = 0x40000000,
		ProtectedDaclSecurityInformation = 0x80000000,
	}
}
