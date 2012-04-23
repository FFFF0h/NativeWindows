using System.Runtime.InteropServices;
using NativeWindows.Processes;

namespace NativeWindows.System
{
	public class SystemInformationFactory : ISystemInformationFactory
	{
		private static class NativeMethods
		{
			[DllImport("kernel32.dll")]
			public static extern void GetSystemInfo(out SystemInformation systemInformation);

			[DllImport("kernel32.dll")]
			public static extern void GetNativeSystemInfo(out SystemInformation systemInformation);
		}

		public SystemInformation GetSystemInfo()
		{
			using (var processHandle = ProcessHandle.GetCurrentProcess())
			{
				SystemInformation systemInformation;
				if (processHandle.IsWow64Process())
				{
					NativeMethods.GetNativeSystemInfo(out systemInformation);
				}
				else
				{
					NativeMethods.GetSystemInfo(out systemInformation);
				}
				return systemInformation;
			}
		}
	}
}
