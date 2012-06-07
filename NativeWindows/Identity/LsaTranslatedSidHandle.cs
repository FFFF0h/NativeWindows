using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace NativeWindows.Identity
{
	public class LsaTranslatedSidHandle : SafeHandle
	{
		[StructLayout(LayoutKind.Sequential)]
		private struct LsaTranslatedSid2
		{
			public SidNameUse Use;
			public IntPtr Sid;
			public int DomainIndex;
			public uint Flags;
		}

		private static class NativeMethods
		{
			[DllImport("advapi32")]
			internal static extern LsaStatus LsaFreeMemory(IntPtr handle);

			[DllImport("advapi32.dll")]
			public static extern bool IsValidSid(IntPtr pSid);
		}

		public LsaTranslatedSidHandle(IntPtr handle, bool ownsHandle = true)
			: base(handle, ownsHandle)
		{
		}

		public IntPtr Sid
		{
			get
			{
				var translatedSid2 = (LsaTranslatedSid2)Marshal.PtrToStructure(handle, typeof(LsaTranslatedSid2));
				IntPtr sid = translatedSid2.Sid;
				if (!NativeMethods.IsValidSid(sid))
				{
					throw new Win32Exception("Invalid SID");
				}
				return sid;
			}
		}

		public SidNameUse Use
		{
			get
			{
				var translatedSid2 = (LsaTranslatedSid2)Marshal.PtrToStructure(handle, typeof(LsaTranslatedSid2));
				return translatedSid2.Use;
			}
		}

		public int DomainIndex
		{
			get
			{
				var translatedSid2 = (LsaTranslatedSid2)Marshal.PtrToStructure(handle, typeof(LsaTranslatedSid2));
				return translatedSid2.DomainIndex;
			}
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
