using NativeWindows.Processes;
using TestProgramWhileTrue;
using Xunit;
using Process = System.Diagnostics.Process;

namespace JobObjectTests
{
	public class ExtendedLimitInformationTest
	{
		[Fact]
		public void GivenJobWithKillOnJobClose_WhenDisposingJob_ThenProcessIsStopped()
		{
			Process process = Process.Start(typeof(Program).Assembly.Location);

			using (var job = JobObjectHandle.Create())
			{
				job.SetInformation(new ExtendedLimitInformation
				{
					KillOnJobClose = true,
				});
				job.AssignProcess(process);
			}

			Assert.True(process.HasExited);
			Assert.Equal(0, process.ExitCode);
		}

		[Fact]
		public void GivenJobObjectWithoutMemoryLimit_ThenOutOfMemoryIsNotThrown()
		{
			Process process = Process.Start(typeof(TestProgramMemoryUsage.Program).Assembly.Location);
			using (var job = JobObjectHandle.Create())
			{
				job.SetInformation(new ExtendedLimitInformation
				{
				});
				job.AssignProcess(process);
			}

			process.WaitForExit();

			Assert.Equal(0, process.ExitCode);
		}

		[Fact]
		public void GivenJobObjectWithMemoryLimit_ThenOutOfMemoryIsThrown()
		{
			Process process = Process.Start(typeof(TestProgramMemoryUsage.Program).Assembly.Location);

			using (var job = JobObjectHandle.Create())
			{
				job.SetInformation(new ExtendedLimitInformation
				{
					ProcessMemoryLimit = 100 * 1024 * 1024,
				});
				job.AssignProcess(process);
			}

			process.WaitForExit();

			Assert.Equal(-1, process.ExitCode);
		}
	}
}
