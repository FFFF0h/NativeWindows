using Microsoft.Win32.SafeHandles;

namespace NativeWindows.IO
{
	public class FileHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		public FileHandle(bool ownsHandle)
			: base(ownsHandle)
		{
		}

		protected override bool ReleaseHandle()
		{
			return true;
		}
	}
}
