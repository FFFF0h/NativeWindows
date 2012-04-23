namespace NativeWindows.Threads
{
	public interface IThreadFactory
	{
		IThread GetCurrentThread();
	}
}
