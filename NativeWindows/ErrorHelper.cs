using System.ComponentModel;
using System.Runtime.InteropServices;

namespace NativeWindows
{
	public static class ErrorHelper
	{
		public static void ThrowCustomWin32Exception()
		{
			var error = (SystemErrorCode)Marshal.GetLastWin32Error();
			throw GetWin32Exception(error);
		}

		public static Win32Exception GetWin32Exception(SystemErrorCode errorCode)
		{
			switch (errorCode)
			{
				case SystemErrorCode.ErrorAccessDenied:
					return new AccessDeniedException();
				default:
					return new Win32Exception((int)errorCode);
			}
		}

		public static string Description(this SystemErrorCode errorCode)
		{
			return new Win32Exception((int)errorCode).Message;
		}

		public static Win32PdhException GetPdhException(int errorCode)
		{
			throw new Win32PdhException(errorCode);
		}
	}
}
