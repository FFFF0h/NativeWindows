using System;
using System.IO;
using System.Threading;
using NativeWindows.Processes;

namespace GetProcessInformation
{
	public class Program
	{
		public static void Main(string[] args)
		{
			string filepath = Path.Combine(typeof(TestProgramWhileTrue.Program).Assembly.Location);
			var factory = new ProcessFactory();

			using (var processInfo = factory.Create(null, filepath, true, ProcessCreationFlags.NewConsole, (IEnvironmentBlock)null, null, new ProcessStartInfo()))
			{
				Thread.Sleep(20000);

				var memoryCounters = processInfo.Process.GetProcessMemoryCounters();
				Console.WriteLine("Memory Counters:");
				Console.WriteLine("----------------");
				Console.WriteLine("- PageFaultCount: {0}", memoryCounters.PageFaultCount);
				Console.WriteLine("- PagefileUsage: {0}", memoryCounters.PagefileUsage);
				Console.WriteLine("- PeakPagefileUsage: {0}", memoryCounters.PeakPagefileUsage);
				Console.WriteLine("- PeakWorkingSetSize: {0}", memoryCounters.PeakWorkingSetSize);
				Console.WriteLine("- PrivateUsage: {0}", memoryCounters.PrivateUsage);
				Console.WriteLine("- QuotaNonPagedPoolUsage: {0}", memoryCounters.QuotaNonPagedPoolUsage);
				Console.WriteLine("- QuotaPagedPoolUsage: {0}", memoryCounters.QuotaPagedPoolUsage);
				Console.WriteLine("- QuotaPeakNonPagedPoolUsage: {0}", memoryCounters.QuotaPeakNonPagedPoolUsage);
				Console.WriteLine("- QuotaPeakPagedPoolUsage: {0}", memoryCounters.QuotaPeakPagedPoolUsage);
				Console.WriteLine("- WorkingSetSize: {0}", memoryCounters.WorkingSetSize);

				processInfo.Process.Terminate(0);

				var processTimes = processInfo.Process.GetProcessTimes();
				Console.WriteLine();
				Console.WriteLine("Process times:");
				Console.WriteLine("--------------");
				Console.WriteLine("- CreationTime: {0}", processTimes.CreationTime);
				Console.WriteLine("- ExitTime: {0}", processTimes.ExitTime);
				Console.WriteLine("- KernelTime: {0}", processTimes.KernelTime);
				Console.WriteLine("- UserTime: {0}", processTimes.UserTime);
			}

			Console.WriteLine("Press any key to quit...");
			Console.ReadKey(intercept: true);
		}
	}
}
