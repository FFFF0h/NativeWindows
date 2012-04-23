namespace NativeWindows.Desktop
{
	public class DesktopFactory : IDesktopFactory
	{
		public IDesktop GetFromCurrentThread()
		{
			var handle = DesktopHandle.GetFromCurrentThread();
			return new Desktop(handle);
		}

		public IDesktop Open(string name, DesktopOpenFlags flags, bool inherit, DesktopAccessRights desiredAccess)
		{
			var handle = DesktopHandle.Open(name, flags, inherit, desiredAccess);
			return new Desktop(handle);
		}
	}
}
