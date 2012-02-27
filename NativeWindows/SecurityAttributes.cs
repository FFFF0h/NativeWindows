using System;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace NativeWindows
{
	[StructLayout(LayoutKind.Sequential)]
	public class SecurityAttributes : IDisposable
	{
		private int _length;
		private IntPtr _securityDescriptor;
		[MarshalAs(UnmanagedType.Bool)]
		private bool _inheritHandle;

		public SecurityAttributes(bool inheritHandle = false)
		{
			_length = Marshal.SizeOf(typeof(SecurityAttributes));
			_securityDescriptor = IntPtr.Zero;
			_inheritHandle = inheritHandle;
		}

		public SecurityAttributes(CommonObjectSecurity objectSecurity)
		{
			_length = Marshal.SizeOf(typeof(SecurityAttributes));
			byte[] src = objectSecurity.GetSecurityDescriptorBinaryForm();
			_securityDescriptor = Marshal.AllocHGlobal(src.Length);
			Marshal.Copy(src, 0, _securityDescriptor, src.Length);
			_inheritHandle = false;
		}

		public void Dispose()
		{
			if (_securityDescriptor != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(_securityDescriptor);
			}
		}
	}
}
