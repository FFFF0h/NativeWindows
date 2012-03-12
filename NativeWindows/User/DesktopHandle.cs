using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace NativeWindows.User
{
	public sealed class DesktopHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		private static class NativeMethods
		{
			[DllImport("user32.dll")]
			public static extern DesktopHandle OpenDesktop(string desktop, DesktopOpenFlags flags, bool inherit, DesktopAccessRights desiredAccess);

			[DllImport("user32.dll", SetLastError = true)]
			public static extern bool CloseDesktop(IntPtr desktop);
		}

		public static DesktopHandle Open(string name, DesktopOpenFlags flags, bool inherit, DesktopAccessRights desiredAccess)
		{
			return NativeMethods.OpenDesktop(name, flags, inherit, desiredAccess);
		}

		public DesktopHandle()
			: base(true)
		{
		}

		protected override bool ReleaseHandle()
		{
			return NativeMethods.CloseDesktop(handle);
		}
	}
}
