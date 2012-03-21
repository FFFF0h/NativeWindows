namespace NativeWindows.IO
{
	public class DirectoryHandle : SafeHandleZeroIsInvalid
	{
		public DirectoryHandle(bool ownsHandle)
			: base(ownsHandle)
		{
		}

		protected override bool ReleaseHandle()
		{
			return true;
		}
	}
}
