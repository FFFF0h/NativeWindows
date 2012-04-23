using System;
using NativeWindows.Security;

namespace NativeWindows.Processes
{
	[Flags]
	public enum JobObjectAccessRights : uint
	{
		None = 0,

		AssignProcess = 1,
		SetAttributes = 2,
		Query = 4,
		Terminate = 8,
		SetSecurityAttributes = 16,

		Delete = 0x00010000,
		ReadPermissions = 0x00020000,
		WritePermissions = 0x00040000,
		TakeOwnership = 0x00080000,
		Synchronize = 0x00100000,

		Read = StandardAccessRights.Read | Query,
		Write = StandardAccessRights.Write | AssignProcess | SetAttributes | Terminate,
		Execute = StandardAccessRights.Execute | Synchronize,

		AllAccess = StandardAccessRights.Required | AssignProcess | SetAttributes | Query | Terminate | SetSecurityAttributes | Synchronize,
	}
}
