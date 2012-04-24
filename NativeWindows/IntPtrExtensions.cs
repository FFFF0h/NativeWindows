using System;
using System.Runtime.InteropServices;

namespace NativeWindows
{
	public static class IntPtrExtensions
	{
		private static class NativeMethods
		{
			[DllImport("kernel32.dll", SetLastError = true)]
			public static extern bool CloseHandle(IntPtr handle);
		}

		public static bool CloseHandle(this IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return true;
			}
			return NativeMethods.CloseHandle(handle);
		}
	}
}
