using System;

namespace NativeWindows.Threads
{
	public interface IThread : IDisposable
	{
		ThreadHandle Handle { get; }

		uint Resume();
		uint Suspend();
	}
}
