using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using NativeWindows.Identity;

namespace NativeWindows.Security
{
	public class SecurityDescriptorHandle : SafeHandle
	{
		private static class NativeMethods
		{
			[DllImport("user32.dll", SetLastError = true)]
			public static extern bool GetUserObjectSecurity(IntPtr handle, ref SecurityInformation siRequested, IntPtr sd, uint length, out uint lengthNeeded);

			[DllImport("advapi32.dll", SetLastError = true)]
			public static extern bool GetSecurityDescriptorOwner(SecurityDescriptorHandle securityDescriptor, out IntPtr handle, out bool ownerDefaulted);
		}

		public static SecurityDescriptorHandle GetUserObjectSecurity(IntPtr handle, SecurityInformation requested)
		{
			uint length;
			var descriptor = IntPtr.Zero;
			if (!NativeMethods.GetUserObjectSecurity(handle, ref requested, descriptor, 0, out length))
			{
				var errorCode = (SystemErrorCode)Marshal.GetLastWin32Error();
				if (errorCode != SystemErrorCode.ErrorInsufficientBuffer)
				{
					throw ErrorHelper.GetWin32Exception(errorCode);
				}
			}

			descriptor = Marshal.AllocHGlobal(new IntPtr(length));
			try
			{
				if (!NativeMethods.GetUserObjectSecurity(handle, ref requested, descriptor, length, out length))
				{
					throw ErrorHelper.GetWin32Exception();
				}
			}
			catch
			{
				Marshal.FreeHGlobal(descriptor);
				throw;
			}

			return new SecurityDescriptorHandle(descriptor);
		}

		private SecurityDescriptorHandle(IntPtr handle, bool ownsHandle = true)
			: base(handle, ownsHandle)
		{
		}

		public SecurityIdentifier GetSecurityDescriptorOwner(out bool ownerDefaulted)
		{
			IntPtr sidPtr;
			if (!NativeMethods.GetSecurityDescriptorOwner(this, out sidPtr, out ownerDefaulted))
			{
				throw ErrorHelper.GetWin32Exception();
			}
			return new SecurityIdentifier(sidPtr);
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

		public override bool IsInvalid
		{
			get
			{
				return handle != IntPtr.Zero;
			}
		}
	}
}
