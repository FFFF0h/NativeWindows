using System;
using System.Runtime.InteropServices;

namespace NativeWindows
{
	public class EventHandle : SafeHandle
	{
		private static class NativeMethods
		{
			[DllImport("coredll.dll", SetLastError = true)]
			public static extern EventHandle CreateEvent(IntPtr eventAttributes, bool manualReset, bool intialState, string name);

			[DllImport("coredll.dll", SetLastError = true)]
			public static extern bool CloseHandle(IntPtr handle);
		}

		public EventHandle()
			: base(IntPtr.Zero, true)
		{
		}

		public EventHandle(IntPtr invalidHandleValue, bool ownsHandle)
			: base(invalidHandleValue, ownsHandle)
		{
		}

		public EventHandle Create(bool manualReset = false, bool initialState = false, string name = null)
		{
			return NativeMethods.CreateEvent(IntPtr.Zero, manualReset, initialState, name);
		}

		protected override bool ReleaseHandle()
		{
			return NativeMethods.CloseHandle(handle);
		}

		public override bool IsInvalid
		{
			get
			{
				return handle == IntPtr.Zero;
			}
		}
	}
}
