using System;
using System.Runtime.InteropServices;

namespace NativeWindows
{
	public class MarshalHGlobal : SafeHandle
	{
		public MarshalHGlobal()
			: base(IntPtr.Zero, true)
		{
		}

		public MarshalHGlobal(int size)
			: this()
		{
			handle = Marshal.AllocHGlobal(size);
		}

		public MarshalHGlobal(uint size)
			: this()
		{
			handle = Marshal.AllocHGlobal(new IntPtr(size));
		}

		public MarshalHGlobal(long size)
			: this()
		{
			handle = Marshal.AllocHGlobal(new IntPtr(size));
		}

		protected override bool ReleaseHandle()
		{
			try
			{
				Marshal.FreeHGlobal(handle);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public IntPtr Pointer
		{
			get
			{
				return handle;
			}
		}

		public T ToStructure<T>()
		{
			return (T)Marshal.PtrToStructure(handle, typeof(T));
		}

		public T ToStructure<T>(long offset)
		{
			var ptr = new IntPtr(handle.ToInt64() + offset);
			return (T)Marshal.PtrToStructure(ptr, typeof(T));
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