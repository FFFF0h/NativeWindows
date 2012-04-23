namespace NativeWindows.Desktop
{
	public class Desktop : IDesktop
	{
		private readonly DesktopHandle _handle;

		public Desktop(DesktopHandle handle)
		{
			_handle = handle;
		}

		public void Dispose()
		{
			_handle.Dispose();
		}

		public DesktopHandle Handle
		{
			get
			{
				return _handle;
			}
		}
	}
}
