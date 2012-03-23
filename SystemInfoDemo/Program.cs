using System;
using NativeWindows.System;

namespace SystemInfoDemo
{
	public class Program
	{
		public static void Main(string[] args)
		{
			SystemInfo system = SystemInformation.GetSystemInfo();
			Console.WriteLine("OemId: {0}", system.OemProcessorArch.OemId);
			Console.WriteLine("ProcessorArch: {0}", system.OemProcessorArch.ProcessorArch);
			Console.WriteLine("ActiveProcessorMask: {0}", system.ActiveProcessorMask.ToUInt64());
			Console.WriteLine("AllocationGranularity: {0}", system.AllocationGranularity);
			Console.WriteLine("MaximumApplicationAddress: {0}", system.MaximumApplicationAddress.ToUInt64());
			Console.WriteLine("MinimumApplicationAddress: {0}", system.MinimumApplicationAddress.ToUInt64());
			Console.WriteLine("NumberOfProcessors: {0}", system.NumberOfProcessors);
			Console.WriteLine("PageSize: {0}", system.PageSize);
			Console.WriteLine("ProcessorLevel: {0}", system.ProcessorLevel);
			Console.WriteLine("ProcessorRevision: {0}", system.ProcessorRevision);
			Console.WriteLine("ProcessorType: {0}", system.ProcessorType);
		}
	}
}
