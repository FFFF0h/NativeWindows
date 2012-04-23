using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using NativeWindows.Processes;
using Process = System.Diagnostics.Process;

namespace JobObjectDemoProgram
{
	public class Program
	{
		public static void Main(string[] args)
		{
			string filepath = Path.Combine(typeof(TestProgramMemoryUsage.Program).Assembly.Location);
			var process1 = Process.Start(filepath);
			var process2 = Process.Start(filepath);

			var security = new JobObjectSecurity(true);
			security.AddAccessRule(new NTAccount("Everyone"), JobObjectAccessRights.AllAccess, AccessControlType.Allow);

			using (var job = JobObjectHandle.Create(security, "Foobar"))
			{
				var extendedLimitInformation = new ExtendedLimitInformation
				{
					KillOnJobClose = true,
					JobMemoryLimit = 3 * 1024 * 1024 * 1024UL,
					ProcessMemoryLimit = 100 * 1024 * 1024,
				};
				job.SetInformation(extendedLimitInformation);

				job.AssignProcess(process1);

				using (var job2 = JobObjectHandle.Open("Foobar"))
				{
					job2.AssignProcess(process2);
					var extendedLimitInformation2 = job2.GetInformation<ExtendedLimitInformation>();
					var basicAccountingInformation = job2.GetInformation<BasicAndIoAccountingInformation>();
					var basicUiRestrictions = job2.GetInformation<BasicUiRestrictions>();
				}

				Console.WriteLine("Press any key to exit...");
				Console.ReadKey(intercept: true);
			}
		}
	}
}
