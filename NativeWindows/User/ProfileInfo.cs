using System;
using System.Runtime.InteropServices;

namespace NativeWindows.User
{
	[StructLayout(LayoutKind.Sequential)]
	public struct ProfileInfo
	{
		public int Size;
		public int Flags;
		[MarshalAs(UnmanagedType.LPTStr)]
		public string Username;
		[MarshalAs(UnmanagedType.LPTStr)]
		public string ProfilePath;
		[MarshalAs(UnmanagedType.LPTStr)]
		public string DefaultPath;
		[MarshalAs(UnmanagedType.LPTStr)]
		public string ServerName;
		[MarshalAs(UnmanagedType.LPTStr)]
		public string PolicyPath;
		public IntPtr Profile;
	}
}
