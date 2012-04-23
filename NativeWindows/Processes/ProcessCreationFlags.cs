using System;

namespace NativeWindows.Processes
{
	[Flags]
	public enum ProcessCreationFlags : uint
	{
		None = 0,
		BreakawayFromJob = 0x01000000,
		DefaultErrorMode = 0x04000000,
		NewConsole = 0x00000010,
		NewProcessGroup = 0x00000200,
		NoWindow = 0x08000000,
		ProtectedProcess = 0x00040000,
		PreserveCodeAuthzLevel = 0x02000000,
		SeparateWowVdm = 0x00000800,
		SharedWowVdm = 0x00001000,
		Suspended = 0x00000004,
		UnicodeEnvironment = 0x00000400,
		DebugOnlyThisProcess = 0x00000002,
		DebugProcess = 0x00000001,
		DetachedProcess = 0x00000008,
		ExtendedStartupinfoPresent = 0x00080000,
		InheritParentAffinity = 0x00010000,
	}
}
