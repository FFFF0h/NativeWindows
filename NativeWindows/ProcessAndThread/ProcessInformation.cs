using System;
using System.Runtime.InteropServices;

namespace NativeWindows.ProcessAndThread
{
	[StructLayout(LayoutKind.Sequential)]
	public class ProcessInformation : IDisposable
	{
		public ProcessHandle ProcessHandle;
		public ThreadHandle ThreadHandle;
		public int ProcessId;
		public int ThreadId;

		public void Dispose()
		{
			ProcessHandle.Dispose();
			ThreadHandle.Dispose();
		}
	}
}
