using System;
using NativeWindows.Security;

namespace NativeWindows.IO
{
	[Flags]
	public enum PipeAccessRights : uint
	{
		None = 0,

		ReadData = 1,
		WriteData = 2,
		CreatePipeInstance = 4,
		ReadAttributes = 128,
		WriteAttributes = 256,

		Delete = 0x00010000,
		ReadPermissions = 0x00020000,
		WritePermissions = 0x00040000,
		TakeOwnership = 0x00080000,
		Synchronize = 0x00100000,

		Read = StandardAccessRights.Read | ReadData | CreatePipeInstance | ReadAttributes,
		Write = StandardAccessRights.Write | WriteData | CreatePipeInstance,
		Execute = StandardAccessRights.Execute,

		AllAccess = StandardAccessRights.Required | ReadData | WriteData | CreatePipeInstance | ReadAttributes | WriteAttributes | Synchronize,
	}
}
