using System;
using System.Runtime.InteropServices;

namespace NativeWindows.System
{
	[StructLayout(LayoutKind.Sequential)]
	public struct SystemInformation
	{
		[StructLayout(LayoutKind.Explicit)]
		public struct OemProcessorArchUnion
		{
			[FieldOffset(0)]
			public uint OemId;

			[FieldOffset(0)]
			public ProcessorArchitecture ProcessorArch;

			[FieldOffset(2)]
			public ushort Reserved;
		}

		public OemProcessorArchUnion OemProcessorArch;
		public uint PageSize;
		public UIntPtr MinimumApplicationAddress;
		public UIntPtr MaximumApplicationAddress;
		public UIntPtr ActiveProcessorMask;
		public uint NumberOfProcessors;
		public uint ProcessorType;
		public uint AllocationGranularity;
		public ushort ProcessorLevel;
		public ushort ProcessorRevision;
	}
}
