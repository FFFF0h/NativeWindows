using System;

namespace NativeWindows.JobObject
{
	[Flags]
	public enum JobObjectAccessRights : int
	{
		Delete = 0x00010000,
		ReadControl = 0x00020000,
		WriteDac = 0x00040000,
		WriteOwner = 0x00080000,
		Synchronize = 0x00100000,

		AssignProcess = 0x00000001,
		SetAttributes = 0x00000002,
		Query = 0x00000004,
		Terminate = 0x00000008,
		SetSecurityAttributes = 0x00000010,

		All = 0x001F001F,
	}
}
