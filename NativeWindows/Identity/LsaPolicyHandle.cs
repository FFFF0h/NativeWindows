using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace NativeWindows.Identity
{
	public class LsaPolicyHandle : SafeHandle
	{
		[StructLayout(LayoutKind.Sequential)]
		private struct LsaUnicodeString : IDisposable
		{
			public LsaUnicodeString(string value)
			{
				Buffer = Marshal.StringToHGlobalUni(value);
				Length = (ushort)(value.Length * UnicodeEncoding.CharSize);
				MaximumLength = (ushort)((value.Length + 1) * UnicodeEncoding.CharSize);
			}

			public void Dispose()
			{
				Marshal.FreeHGlobal(Buffer);
			}

			public ushort Length;
			public ushort MaximumLength;
			public IntPtr Buffer;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct LsaObjectAttributes
		{
			public int Length;
			public IntPtr RootDirectory;
			public LsaUnicodeString ObjectName;
			public UInt32 Attributes;
			public IntPtr SecurityDescriptor;
			public IntPtr SecurityQualityOfService;
		}

		private static class NativeMethods
		{
			[DllImport("advapi32.dll", PreserveSig = true)]
			public static extern LsaStatus LsaOpenPolicy(ref LsaUnicodeString systemName, ref LsaObjectAttributes objectAttributes, int desiredAccess, out LsaPolicyHandle policyHandle);

			[DllImport("advapi32.dll")]
			public static extern LsaStatus LsaClose(IntPtr objectHandle);

			[DllImport("advapi32.dll")]
			public static extern uint LsaNtStatusToWinError(LsaStatus status);

			[DllImport("advapi32.dll", SetLastError = true, PreserveSig = true)]
			public static extern LsaStatus LsaAddAccountRights(LsaPolicyHandle policyHandle, IntPtr accountSid, LsaUnicodeString[] userRights, uint countOfRights);

			[DllImport("advapi32", CharSet = CharSet.Unicode, SetLastError = true)]
			internal static extern LsaStatus LsaLookupNames2(LsaPolicyHandle policyHandle, LsaLookupNamesFlags flags, uint count, LsaUnicodeString[] names, out LsaReferencedDomainsHandle referencedReferencedDomains, out LsaTranslatedSidHandle translatedSid);
		}

		public static LsaPolicyHandle Open(LsaAccessPolicy accessPolicy)
		{
			var systemName = new LsaUnicodeString();
			var objectAttributes = new LsaObjectAttributes
			{
				Length = 0,
				RootDirectory = IntPtr.Zero,
				Attributes = 0,
				SecurityDescriptor = IntPtr.Zero,
				SecurityQualityOfService = IntPtr.Zero,
			};

			LsaPolicyHandle handle = null;
			LsaChecked(() => NativeMethods.LsaOpenPolicy(ref systemName, ref objectAttributes, (int)accessPolicy, out handle));
			return handle;
		}

		public LsaPolicyHandle(IntPtr invalidHandleValue, bool ownsHandle) : base(invalidHandleValue, ownsHandle)
		{
		}

		public void AddRights(LsaTranslatedSidHandle translatedSidHandle, params string[] userRights)
		{
			var rights = new LsaUnicodeString[userRights.Length];
			for (int i = 0; i < userRights.Length; i++)
			{
				rights[i] = new LsaUnicodeString(userRights[i]);
			}

			try
			{
				LsaChecked(() => NativeMethods.LsaAddAccountRights(this, translatedSidHandle.Sid, rights, (uint)rights.Length));
			}
			finally
			{
				rights.DisposeAll();
			}
		}

		public void AddRights(string name, params string[] userRights)
		{
			using (var domainAndSid = LookupNames2(name))
			{
				AddRights(domainAndSid.TranslatedSidHandle, userRights);
			}
		}

		public LsaNamesResult LookupNames2(string name, LsaLookupNamesFlags flags = LsaLookupNamesFlags.None)
		{
			using (var lsaString = new LsaUnicodeString(name))
			{
				var names = new[] { lsaString };
				LsaReferencedDomainsHandle referencedDomainsHandle = null;
				LsaTranslatedSidHandle translatedSidHandle = null;
				LsaChecked(() => NativeMethods.LsaLookupNames2(this, flags, 1, names, out referencedDomainsHandle, out translatedSidHandle));
				return new LsaNamesResult(referencedDomainsHandle, translatedSidHandle);
			}
		}

		protected override bool ReleaseHandle()
		{
			return NativeMethods.LsaClose(handle) == LsaStatus.Success;
		}

		public override bool IsInvalid
		{
			get
			{
				return handle == IntPtr.Zero;
			}
		}

		private static void LsaChecked(Func<LsaStatus> func)
		{
			LsaStatus status = func();
			if (status != LsaStatus.Success)
			{
				throw new Win32Exception((int)NativeMethods.LsaNtStatusToWinError(status));
			}
		}
	}
}
