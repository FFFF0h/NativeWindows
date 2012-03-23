using System.Runtime.InteropServices;
using NativeWindows.ProcessAndThread;

namespace NativeWindows.System
{
	public static class SystemInformation
	{
		private static class NativeMethods
		{
			[DllImport("kernel32.dll")]
			public static extern void GetSystemInfo(out SystemInfo systemInfo);

			[DllImport("kernel32.dll")]
			public static extern void GetNativeSystemInfo(out SystemInfo systemInfo);
		}

		public static SystemInfo GetSystemInfo()
		{
			using (var processHandle = ProcessHandle.GetCurrentProcess())
			{
				SystemInfo systemInfo;
				if (processHandle.IsWow64Process())
				{
					NativeMethods.GetNativeSystemInfo(out systemInfo);
				}
				else
				{
					NativeMethods.GetSystemInfo(out systemInfo);
				}
				return systemInfo;
			}
		}
	}
}
