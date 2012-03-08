using System;

namespace NativeWindows.ProcessAndThread
{
	// http://msdn.microsoft.com/en-us/library/windows/desktop/ms686769(v=vs.85).aspx
	[Flags]
	public enum ThreadAccessRights : uint
	{
		None = 0,

		Terminate = 1,
		SuspendResume = 2,
		GetContext = 8,
		SetContext = 16,
		SetInformation = 32,
		QueryInformation = 64,
		SetThreadToken = 128,
		Impersonate = 256,
		DirectImpersonation = 512,

		Delete = 0x00010000,
		ReadPermissions = 0x00020000,
		WritePermissions = 0x00040000,
		TakeOwnership = 0x00080000,
		Synchronize = 0x00100000,

		Read = StandardAccessRights.Read | GetContext | QueryInformation,
		Write = StandardAccessRights.Write | Terminate | SuspendResume | SetInformation | SetContext,
		Execute = StandardAccessRights.Execute | Synchronize,

		AllAccess = StandardAccessRights.Required | Synchronize | Terminate | SuspendResume | GetContext | SetContext | SetInformation | QueryInformation | SetThreadToken | Impersonate | DirectImpersonation,
	}
}
