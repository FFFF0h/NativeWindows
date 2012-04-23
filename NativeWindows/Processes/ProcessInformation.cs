using System;
using NativeWindows.Threads;

namespace NativeWindows.Processes
{
	public class ProcessInformation : IProcessInformation
	{
		private readonly IProcess _process;
		private readonly int _processId;
		private readonly IThread _thread;
		private readonly int _threadId;

		public ProcessInformation(IntPtr processHandle, int processId, IntPtr threadHandle, int threadId)
		{
			_process = new Process(new ProcessHandle(processHandle));
			_processId = processId;
			_thread = new Thread(new ThreadHandle(threadHandle));
			_threadId = threadId;
		}

		public void Dispose()
		{
			_process.Dispose();
			_thread.Dispose();
		}

		public IProcess Process
		{
			get
			{
				return _process;
			}
		}

		public int ProcessId
		{
			get
			{
				return _processId;
			}
		}

		public IThread Thread
		{
			get
			{
				return _thread;
			}
		}

		public int ThreadId
		{
			get
			{
				return _threadId;
			}
		}
	}
}
