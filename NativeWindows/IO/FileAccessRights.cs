using System;

namespace NativeWindows.IO
{
	// http://msdn.microsoft.com/en-us/library/windows/desktop/gg258116(v=vs.85).aspx
	// http://msdn.microsoft.com/en-us/library/windows/desktop/aa364399(v=vs.85).aspx
	[Flags]
	public enum FileAccessRights : uint
	{
		None = 0,

		ReadData = 1,
		WriteData = 2,
		AppendData = 4,
		ReadEa = 8,
		WriteEa = 16,
		ExecuteFile = 32,
		DeleteChild = 64,
		ReadAttributes = 128,
		WriteAttributes = 256,

		Delete = 0x00010000,
		ReadPermissions = 0x00020000,
		WritePermissions = 0x00040000,
		TakeOwnership = 0x00080000,
		Synchronize = 0x00100000,

		Read = StandardAccessRights.Read | ReadData | ReadAttributes | ReadEa | Synchronize,
		Write = StandardAccessRights.Write | WriteData | WriteAttributes | WriteEa | AppendData | Synchronize,
		Execute = StandardAccessRights.Execute | ReadAttributes | ExecuteFile | Synchronize,

		AllAccess = StandardAccessRights.Required | Synchronize | ReadData | ReadAttributes | ReadEa | WriteData | WriteAttributes | WriteEa | AppendData | ExecuteFile | DeleteChild,
	}
}
