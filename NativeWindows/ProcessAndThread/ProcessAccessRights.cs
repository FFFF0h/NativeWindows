using System;

namespace NativeWindows.ProcessAndThread
{
	// http://msdn.microsoft.com/en-us/library/windows/desktop/ms684880(v=vs.85).aspx
	[Flags]
	public enum ProcessAccessRights : uint
	{
		None = 0,

		Terminate = 1,
		CreateThread = 2,
		SetSessionId = 4,
		VmOperation = 8,
		VmRead = 16,
		VmWrite = 32,
		DuplicateHandle = 64,
		CreateProcess = 128,
		SetQuota = 256,
		SetInformation = 512,
		QueryInformation = 1024,
		SuspendResume = 2048,

		Delete = 0x00010000,
		ReadPermissions = 0x00020000,
		WritePermissions = 0x00040000,
		TakeOwnership = 0x00080000,
		Synchronize = 0x00100000,

		Read = StandardAccessRights.Read | VmRead | QueryInformation,
		Write = StandardAccessRights.Write | CreateProcess | CreateThread | VmOperation | VmWrite | DuplicateHandle | Terminate | SetQuota | SetInformation,
		Execute = StandardAccessRights.Execute | Synchronize,

		AllAccess = StandardAccessRights.Required | Synchronize | Terminate | CreateThread | SetSessionId | VmOperation | VmRead | VmWrite | DuplicateHandle | CreateProcess | SetQuota | SetInformation | QueryInformation | SuspendResume,
	}
}
