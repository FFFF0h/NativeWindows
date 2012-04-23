namespace NativeWindows.Threads
{
	public class Thread : IThread
	{
		private readonly ThreadHandle _handle;

		public Thread(ThreadHandle handle)
		{
			_handle = handle;
		}

		public void Dispose()
		{
			_handle.Dispose();
		}

		public ThreadHandle Handle
		{
			get
			{
				return _handle;
			}
		}

		public uint Resume()
		{
			return _handle.Resume();
		}

		public uint Suspend()
		{
			return _handle.Suspend();
		}
	}
}
