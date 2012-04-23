using System;
using NativeWindows.Threads;

namespace NativeWindows.Processes
{
	public interface IProcessInformation : IDisposable
	{
		IProcess Process { get; }
		int ProcessId { get; }
		IThread Thread { get; }
		int ThreadId { get; }
	}
}
