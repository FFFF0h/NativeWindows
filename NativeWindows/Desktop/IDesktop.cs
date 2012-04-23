using System;

namespace NativeWindows.Desktop
{
	public interface IDesktop : IDisposable
	{
		DesktopHandle Handle { get; }
	}
}
