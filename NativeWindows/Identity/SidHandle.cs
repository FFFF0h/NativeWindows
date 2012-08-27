using System;
using System.Runtime.InteropServices;
using System.Text;

namespace NativeWindows.Identity
{
	public class SidHandle : SafeHandle
	{
		private static class NativeMethods
		{
			[DllImport("advapi32.dll", SetLastError = true)]
			public static extern bool LookupAccountSid(string systemName, SidHandle sidHandle, StringBuilder name, ref uint nameSize, StringBuilder referencedDomainName, ref uint referencedDomainNameSize, out SidNameUse use);

			[DllImport("advapi32.dll", SetLastError = true)]
			public static extern bool IsValidSid(SidHandle sid);
		}

		public SidHandle(IntPtr invalidHandleValue, bool ownsHandle = true)
			: base(invalidHandleValue, ownsHandle)
		{
		}

		protected override bool ReleaseHandle()
		{
			// Unsure if the Sid pointer should be freed. There are no documentation
			return true;
		}

		public override bool IsInvalid
		{
			get
			{
				return handle != IntPtr.Zero;
			}
		}

		public string GetUsername(string systemName = null)
		{
			var name = new StringBuilder();
			uint nameSize = (uint)name.Capacity;
			var domain = new StringBuilder();
			uint domainSize = (uint)domain.Capacity;
			SidNameUse use;

			if (NativeMethods.LookupAccountSid(systemName, this, name, ref nameSize, domain, ref domainSize, out use))
			{
				return name.ToString();
			}

			var error = (SystemErrorCode)Marshal.GetLastWin32Error();
			if (error != SystemErrorCode.ErrorInsufficientBuffer)
			{
				throw ErrorHelper.GetWin32Exception(error);
			}

			name.EnsureCapacity((int)nameSize);
			domain.EnsureCapacity((int)domainSize);
			if (!NativeMethods.LookupAccountSid(systemName, this, name, ref nameSize, domain, ref domainSize, out use))
			{
				throw ErrorHelper.GetWin32Exception();
			}

			return name.ToString();
		}

		public bool IsValid()
		{
			return NativeMethods.IsValidSid(this);
		}
	}
}
