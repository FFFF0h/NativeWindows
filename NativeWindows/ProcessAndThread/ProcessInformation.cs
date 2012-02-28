using System;
using System.Runtime.InteropServices;

namespace NativeWindows.ProcessAndThread
{
	[StructLayout(LayoutKind.Sequential)]
	public class ProcessInformation : IDisposable
	{
		public ProcessInformation(IntPtr processHandle, int processId, IntPtr threadHandle, int threadId)
		{
			ProcessHandle = new ProcessHandle(processHandle);
			ProcessId = processId;
			ThreadHandle = new ThreadHandle(threadHandle);
			ThreadId = threadId;
		}

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
