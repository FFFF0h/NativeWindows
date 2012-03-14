using System;
using System.Runtime.InteropServices;
using NativeWindows.ErrorHandling;

namespace NativeWindows.User
{
	public class WindowStationHandle : SafeHandle
	{
		private static class NativeMethods
		{
			[DllImport("user32", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern IntPtr GetProcessWindowStation();

			[DllImport("user32", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern bool CloseWindowStation(IntPtr handle);
		}

		public static WindowStationHandle GetProcessWindowStation()
		{
			IntPtr handle = NativeMethods.GetProcessWindowStation();
			if (handle == IntPtr.Zero)
			{
				ErrorHelper.ThrowCustomWin32Exception();
			}
			return new WindowStationHandle(handle, false);
		}

		public WindowStationHandle(IntPtr invalidHandleValue, bool ownsHandle)
			: base(invalidHandleValue, ownsHandle)
		{
		}

		protected override bool ReleaseHandle()
		{
			return NativeMethods.CloseWindowStation(handle);
		}

		public override bool IsInvalid
		{
			get
			{
				return handle == IntPtr.Zero || handle == new IntPtr(-1);
			}
		}
	}
}
