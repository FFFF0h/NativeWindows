using System;
using System.Threading.Tasks;

namespace NativeWindows.Processes
{
	public interface IProcess : IDisposable
	{
		ProcessHandle Handle { get; }

		bool HasExited { get; }
		Task<int> Completion { get; }

		bool IsProcessInJob();
		bool IsProcessInJob(IJobObject jobObject);
		bool IsProcessInJob(JobObjectHandle jobObject);
		bool IsWow64Process();
		int GetProcessId();
		void Terminate(int exitCode);
		int GetExitCode();
		bool WaitForExit(TimeSpan timeout);
	}
}
