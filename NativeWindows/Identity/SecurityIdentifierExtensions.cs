using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace NativeWindows.Identity
{
	public static class SecurityIdentifierExtensions
	{
		private static class NativeMethods
		{
			[DllImport("advapi32.dll", SetLastError = true)]
			public static extern bool LookupAccountSid(string systemName, IntPtr sidHandle, StringBuilder name, ref uint nameSize, StringBuilder referencedDomainName, ref uint referencedDomainNameSize, out SidNameUse use);

			[DllImport("advapi32.dll", SetLastError = true)]
			public static extern bool IsValidSid(IntPtr sid);
		}

		private static byte[] GetBinaryForm(SecurityIdentifier securityIdentifier)
		{
			var data = new byte[securityIdentifier.BinaryLength];
			securityIdentifier.GetBinaryForm(data, 0);
			return data;
		}

		public static unsafe bool IsValid(this SecurityIdentifier securityIdentifier)
		{
			fixed (void* ptr = GetBinaryForm(securityIdentifier))
			{
				var sid = new IntPtr(ptr);
				return NativeMethods.IsValidSid(sid);
			}
		}

		public static unsafe string GetUsername(this SecurityIdentifier securityIdentifier, string systemName = null)
		{
			var name = new StringBuilder();
			uint nameSize = (uint)name.Capacity;
			var domain = new StringBuilder();
			uint domainSize = (uint)domain.Capacity;
			SidNameUse use;

			fixed (void* ptr = GetBinaryForm(securityIdentifier))
			{
				var sid = new IntPtr(ptr);

				if (NativeMethods.LookupAccountSid(systemName, sid, name, ref nameSize, domain, ref domainSize, out use))
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
				if (!NativeMethods.LookupAccountSid(systemName, sid, name, ref nameSize, domain, ref domainSize, out use))
				{
					throw ErrorHelper.GetWin32Exception();
				}
			}

			return name.ToString();
		}
	}
}
