using System.Runtime.InteropServices;

namespace NativeWindows.ProcessAndThread
{
	[StructLayout(LayoutKind.Sequential)]
	public class ProcessInformation
	{
		public ProcessHandle ProcessHandle;
		public ThreadHandle ThreadHandle;
		public int ProcessId;
		public int ThreadId;
	}
}
