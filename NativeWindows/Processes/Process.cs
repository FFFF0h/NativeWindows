using System;
using System.Threading.Tasks;

namespace NativeWindows.Processes
{
	public class Process : IProcess
	{
		private readonly ProcessHandle _handle;

		public Process(ProcessHandle handle)
		{
			_handle = handle;
		}

		public void Dispose()
		{
			_handle.Dispose();
		}

		public ProcessHandle Handle
		{
			get
			{
				return _handle;
			}
		}

		public bool HasExited
		{
			get
			{
				return _handle.HasExited;
			}
		}

		public Task<int> Completion
		{
			get
			{
				return _handle.Completion;
			}
		}

		public bool IsProcessInJob()
		{
			return _handle.IsProcessInJob();
		}

		public bool IsProcessInJob(IJobObject jobObject)
		{
			return _handle.IsProcessInJob(jobObject.Handle);
		}

		public bool IsProcessInJob(JobObjectHandle jobObject)
		{
			return _handle.IsProcessInJob(jobObject);
		}

		public bool IsWow64Process()
		{
			return _handle.IsWow64Process();
		}

		public int GetProcessId()
		{
			return _handle.GetProcessId();
		}

		public void Terminate(int exitCode)
		{
			_handle.Terminate(exitCode);
		}

		public int GetExitCode()
		{
			return _handle.GetExitCode();
		}

		public bool WaitForExit(TimeSpan timeout)
		{
			return _handle.WaitForExit(timeout);
		}

		public ProcessMemoryCounters GetProcessMemoryCounters()
		{
			return _handle.GetProcessMemoryCounters();
		}
	}
}
