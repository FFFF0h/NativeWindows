using System;

namespace NativeWindows.Processes
{
	public interface IJobObject : IDisposable
	{
		JobObjectHandle Handle { get; }

		void AssignProcess(global::System.Diagnostics.Process process);
		void AssignProcess(IProcess process);
		void AssignProcess(ProcessHandle processHandle);
		T GetInformation<T>() where T : IJobObjectQueryable, new();
		void SetInformation<T>(T jobObjectStructureWrapper) where T : IJobObjectSettable;
		void Terminate(int exitCode);
	}
}
