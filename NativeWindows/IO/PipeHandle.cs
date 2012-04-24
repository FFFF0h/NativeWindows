namespace NativeWindows.IO
{
	public class PipeHandle : SafeHandleZeroIsInvalid
	{
		public PipeHandle(bool ownsHandle)
			: base(ownsHandle)
		{
		}

		protected override bool ReleaseHandle()
		{
			return true;
		}
	}
}
