using System;
using System.Runtime.InteropServices;

namespace NativeWindows.Identity
{
	public class LsaReferencedDomainsHandle : SafeHandle
	{
		private static class NativeMethods
		{
			[DllImport("advapi32")]
			internal static extern LsaStatus LsaFreeMemory(IntPtr handle);
		}

		public LsaReferencedDomainsHandle(IntPtr handle, bool ownsHandle = true)
			: base(handle, ownsHandle)
		{
		}

		protected override bool ReleaseHandle()
		{
			return NativeMethods.LsaFreeMemory(handle) == LsaStatus.Success;
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
