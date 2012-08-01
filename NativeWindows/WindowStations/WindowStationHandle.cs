using System;
using System.Runtime.InteropServices;

namespace NativeWindows.WindowStations
{
	public class WindowStationHandle : SafeHandle
	{
		private static class NativeMethods
		{
			[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern IntPtr GetProcessWindowStation();

			[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern bool CloseWindowStation(IntPtr handle);

			[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern IntPtr OpenWindowStation(string name, bool inheritHandle, WindowStationAccessRights desiredAccess);

			[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern bool SetProcessWindowStation(WindowStationHandle handle);
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

		public static WindowStationHandle Open(string name, bool inheritHandle, WindowStationAccessRights desiredAccess)
		{
			var handle = new WindowStationHandle(NativeMethods.OpenWindowStation(name, inheritHandle, desiredAccess), true);
			if (handle.IsInvalid)
			{
				ErrorHelper.ThrowCustomWin32Exception();
			}
			return handle;
		}

		public WindowStationHandle(IntPtr invalidHandleValue, bool ownsHandle)
			: base(invalidHandleValue, ownsHandle)
		{
		}

		public void SetProcessWindowStation()
		{
			if (!NativeMethods.SetProcessWindowStation(this))
			{
				ErrorHelper.ThrowCustomWin32Exception();
			}
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
