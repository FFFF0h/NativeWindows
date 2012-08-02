using System;

namespace NativeWindows.Registry
{
	public class HKey
	{
		public static readonly UIntPtr HKEY_CLASSES_ROOT = new UIntPtr(0x80000000);
		public static readonly UIntPtr HKEY_CURRENT_USER = new UIntPtr(0x80000001);
		public static readonly UIntPtr HKEY_LOCAL_MACHINE = new UIntPtr(0x80000002);
		public static readonly UIntPtr HKEY_USERS = new UIntPtr(0x80000003);
		public static readonly UIntPtr HKEY_PERFORMANCE_DATA = new UIntPtr(0x80000004);
		public static readonly UIntPtr HKEY_CURRENT_CONFIG = new UIntPtr(0x80000005);
		public static readonly UIntPtr HKEY_DYN_DATA = new UIntPtr(0x80000006);
		public static readonly UIntPtr HKEY_CURRENT_USER_LOCAL_SETTINGS = new UIntPtr(0x80000007);
		public static readonly UIntPtr HKEY_PERFORMANCE_TEXT = new UIntPtr(0x80000050);
		public static readonly UIntPtr HKEY_PERFORMANCE_NLSTEXT = new UIntPtr(0x80000060);
	}
}
