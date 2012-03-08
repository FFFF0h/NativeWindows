using System;

namespace NativeWindows.IO
{
	[Flags]
	public enum DirectoryAccessRights : uint
	{
		None = 0,

		ListDirectory = 1,
		AddFile = 2,
		AddSubdirectory = 4,
		ReadEa = 8,
		WriteEa = 16,
		Traverse = 32,
		DeleteChild = 64,
		ReadAttributes = 128,
		WriteAttributes = 256,

		Delete = 0x00010000,
		ReadPermissions = 0x00020000,
		WritePermissions = 0x00040000,
		TakeOwnership = 0x00080000,
		Synchronize = 0x00100000,

		Read = StandardAccessRights.Read | ListDirectory | AddFile,
		Write = StandardAccessRights.Write | AddSubdirectory | ReadEa,
		Execute = StandardAccessRights.Execute | ListDirectory | AddFile,

		AllAccess = StandardAccessRights.Required | ListDirectory | AddFile | AddSubdirectory | ReadEa,
	}
}
