using System.ComponentModel;

namespace NativeWindows
{
	public class Win32PdhException : Win32Exception
	{
		public Win32PdhException(int errorCode)
			: base(errorCode)
		{
		}
	}
}
