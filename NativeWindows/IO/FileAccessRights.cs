using System;

namespace NativeWindows.IO
{
	[Flags]
	public enum FileAccessRights : uint
	{
		None = 0,

		AccessSystemSecurity = 0x1000000,
		MaximumAllowed = 0x2000000,

		Delete = 0x10000,
		ReadControl = 0x20000,
		WriteDac = 0x40000,
		WriteOwner = 0x80000,
		Synchronize = 0x100000,

		StandardRightsRequired = 0xF0000,
		StandardRightsRead = ReadControl,
		StandardRightsWrite = ReadControl,
		StandardRightsExecute = ReadControl,
		StandardRightsAll = 0x1F0000,
		SpecificRightsAll = 0xFFFF,

		// http://msdn.microsoft.com/en-us/library/windows/desktop/gg258116(v=vs.85).aspx
		FileReadData = 0x0001,
		FileListDirectory = 0x0001,
		FileWriteData = 0x0002,
		FileAddFile = 0x0002,
		FileAppendData = 0x0004,
		FileAddSubdirectory = 0x0004,
		FileCreatePipeInstance = 0x0004,
		FileReadEa = 0x0008,
		FileWriteEa = 0x0010,
		FileExecute = 0x0020,
		FileTraverse = 0x0020,
		FileDeleteChild = 0x0040,
		FileReadAttributes = 0x0080,
		FileWriteAttributes = 0x0100,

		GenericRead = 0x80000000,
		GenericWrite = 0x40000000,
		GenericExecute = 0x20000000,
		GenericAll = 0x10000000,

		// http://msdn.microsoft.com/en-us/library/windows/desktop/aa364399(v=vs.85).aspx
		FileAllAccess = StandardRightsRequired | Synchronize | 0x1FF,
		FileGenericRead = StandardRightsRead | FileReadData | FileReadAttributes | FileReadEa | Synchronize,
		FileGenericWrite = StandardRightsWrite | FileWriteData | FileWriteAttributes | FileWriteEa | FileAppendData | Synchronize,
		FileGenericExecute = StandardRightsExecute | FileReadAttributes | FileExecute | Synchronize,
	}
}
