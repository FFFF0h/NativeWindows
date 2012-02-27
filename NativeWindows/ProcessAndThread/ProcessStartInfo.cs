using System.Runtime.InteropServices;
using NativeWindows.IO;

namespace NativeWindows.ProcessAndThread
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public class ProcessStartInfo
	{
		public int cb = Marshal.SizeOf(typeof(ProcessStartInfo));
		private string Reserved;
		public string Desktop;
		public string Title;
		public int X;
		public int Y;
		public int XSize;
		public int YSize;
		public int XCountChars;
		public int YCountChars;
		public int FillAttribute;
		public ProcessStartInfoFlags Flags;
		public ushort ShowWindow;
		private ushort Reserved2;
		private byte Reserved3;
		public FileHandle StdInput;
		public FileHandle StdOutput;
		public FileHandle StdError;
	}
}
