namespace NativeWindows.Desktop
{
	public interface IDesktopFactory
	{
		IDesktop GetFromCurrentThread();
		IDesktop Open(string name, DesktopOpenFlags flags, bool inherit, DesktopAccessRights desiredAccess);
	}
}
