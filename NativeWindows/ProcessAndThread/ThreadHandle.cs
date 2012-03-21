using System;
using System.Runtime.InteropServices;
using NativeWindows.ErrorHandling;

namespace NativeWindows.ProcessAndThread
{
	/// <summary>
	/// ThreadHandle is completly inconsistent, -1 is a valid handle and means current Thread
	/// </summary>
	public class ThreadHandle : SafeHandleZeroIsInvalid
	{
		private static class NativeMethods
		{
			[DllImport("kernel32.dll", SetLastError = true)]
			public static extern int ResumeThread(ThreadHandle threadHandle);

			[DllImport("kernel32.dll", SetLastError = true)]
			public static extern int SuspendThread(ThreadHandle threadHandle);

			[DllImport("kernel32.dll", SetLastError = true)]
			public static extern ThreadHandle GetCurrentThread();
		}

		public static ThreadHandle GetCurrentThread()
		{
			return NativeMethods.GetCurrentThread();
		}

		public ThreadHandle()
			: base(true)
		{
		}

		public ThreadHandle(IntPtr handle, bool ownsHandle = true)
			: base(ownsHandle)
		{
			SetHandle(handle);
		}

		public uint Resume()
		{
			int result = NativeMethods.ResumeThread(this);
			if (result == -1)
			{
				ErrorHelper.ThrowCustomWin32Exception();
			}
			return (uint)result;
		}

		public uint Suspend()
		{
			int result = NativeMethods.SuspendThread(this);
			if (result == -1)
			{
				ErrorHelper.ThrowCustomWin32Exception();
			}
			return (uint)result;
		}

		protected override bool ReleaseHandle()
		{
			if (handle != new IntPtr(-1))
			{
				return handle.CloseHandle();
			}
			return true;
		}
	}
}
