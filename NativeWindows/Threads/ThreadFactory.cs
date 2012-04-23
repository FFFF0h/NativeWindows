namespace NativeWindows.Threads
{
	public class ThreadFactory : IThreadFactory
	{
		public IThread GetCurrentThread()
		{
			var handle = ThreadHandle.GetCurrentThread();
			return new Thread(handle);
		}
	}
}
