using System.ComponentModel;
using System.Runtime.InteropServices;

namespace NativeWindows.Exceptions
{
	public static class ErrorHelper
	{
		public static void ThrowCustomWin32Exception()
		{
			var error = (SystemErrorCode)Marshal.GetLastWin32Error();
			switch (error)
			{
				case SystemErrorCode.ErrorAccessDenied:
					throw new AccessDeniedException();
				default:
					throw new Win32Exception();
			}
		}

		public static string Description(this SystemErrorCode errorCode)
		{
			return new Win32Exception((int)errorCode).Message;
		}
	}
}
