using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using NativeWindows.ProcessAndThread;

namespace NativeWindows.User
{
	public sealed class DesktopHandle : SafeHandle
	{
		private static class NativeMethods
		{
			[DllImport("user32.dll")]
			public static extern DesktopHandle OpenDesktop(string desktop, DesktopOpenFlags flags, bool inherit, DesktopAccessRights desiredAccess);

			[DllImport("user32.dll", SetLastError = true)]
			public static extern bool CloseDesktop(IntPtr desktop);

			[DllImport("user32.dll", SetLastError = true)]
			public static extern IntPtr GetThreadDesktop(ThreadHandle threadHandle);
		}

		public static DesktopHandle GetFromCurrentThread()
		{
			using(var handle = ThreadHandle.GetCurrentThread())
			{
				return GetFromThread(handle);
			}
		}

		public static DesktopHandle GetFromThread(ThreadHandle threadHandle)
		{
			var handle = NativeMethods.GetThreadDesktop(threadHandle);
			if (handle == IntPtr.Zero)
			{
				throw new Win32Exception();
			}
			return new DesktopHandle(handle);
		}

		public static DesktopHandle Open(string name, DesktopOpenFlags flags, bool inherit, DesktopAccessRights desiredAccess)
		{
			return NativeMethods.OpenDesktop(name, flags, inherit, desiredAccess);
		}

		public DesktopHandle()
			: base(IntPtr.Zero, true)
		{
		}

		private DesktopHandle(IntPtr handle, bool ownsHandle = false)
			: base(handle, ownsHandle)
		{
		}

		protected override bool ReleaseHandle()
		{
			return NativeMethods.CloseDesktop(handle);
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
