using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace NativeWindows.ProcessAndThread
{
	/// <summary>
	/// ThreadHandle is completly inconsistent, -1 is a valid handle and means current Thread
	/// </summary>
	public class ThreadHandle : SafeHandleZeroIsInvalid
	{
		private static class NativeMethods
		{
			[DllImport("kernel32.dll")]
			public static extern int ResumeThread(ThreadHandle threadHandle);

			[DllImport("kernel32.dll")]
			public static extern int SuspendThread(ThreadHandle threadHandle);
		}

		public ThreadHandle()
			: base(true)
		{
		}

		public uint Resume()
		{
			int result = NativeMethods.ResumeThread(this);
			if (result == -1)
			{
				throw new Win32Exception();
			}
			return (uint) result;
		}

		public uint Suspend()
		{
			int result = NativeMethods.SuspendThread(this);
			if (result == -1)
			{
				throw new Win32Exception();
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
