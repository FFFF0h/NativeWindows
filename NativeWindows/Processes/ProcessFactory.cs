using NativeWindows.Identity;
using NativeWindows.Threads;

namespace NativeWindows.Processes
{
	public class ProcessFactory : IProcessFactory
	{
		public IProcessInformation Create(string applicationName, string commandLine, bool inheritHandles, ProcessCreationFlags creationFlags, IEnvironmentBlock environment, string currentDirectory, ProcessStartInfo startInfo, ProcessSecurity processSecurity = null, ThreadSecurity threadSecurity = null)
		{
			return Create(applicationName, commandLine, inheritHandles, creationFlags, environment == null ? null : environment.Handle, currentDirectory, startInfo, processSecurity, threadSecurity);
		}

		public IProcessInformation Create(string applicationName, string commandLine, bool inheritHandles, ProcessCreationFlags creationFlags, EnvironmentBlockHandle environment, string currentDirectory, ProcessStartInfo startInfo, ProcessSecurity processSecurity = null, ThreadSecurity threadSecurity = null)
		{
			return ProcessHandle.Create(applicationName, commandLine, inheritHandles, creationFlags, environment, currentDirectory, startInfo, processSecurity, threadSecurity);
		}

		public IProcessInformation CreateAsUser(IToken token, string applicationName, string commandLine, bool inheritHandles, ProcessCreationFlags creationFlags, IEnvironmentBlock environment, string currentDirectory, ProcessStartInfo startInfo, ProcessSecurity processSecurity = null, ThreadSecurity threadSecurity = null)
		{
			return CreateAsUser(token.Handle, applicationName, commandLine, inheritHandles, creationFlags, environment == null ? null : environment.Handle, currentDirectory, startInfo, processSecurity, threadSecurity);
		}

		public IProcessInformation CreateAsUser(TokenHandle token, string applicationName, string commandLine, bool inheritHandles, ProcessCreationFlags creationFlags, EnvironmentBlockHandle environment, string currentDirectory, ProcessStartInfo startInfo, ProcessSecurity processSecurity = null, ThreadSecurity threadSecurity = null)
		{
			return ProcessHandle.CreateAsUser(token, applicationName, commandLine, inheritHandles, creationFlags, environment, currentDirectory, startInfo, processSecurity, threadSecurity);
		}

		public IProcessInformation CreateWithLogin(string username, string domain, string password, ProcessLogonFlags logonFlags, string applicationName, string commandLine, ProcessCreationFlags creationFlags, IEnvironmentBlock environment, string currentDirectory, ProcessStartInfo startupInfo)
		{
			return CreateWithLogin(username, domain, password, logonFlags, applicationName, commandLine, creationFlags, environment == null ? null : environment.Handle, currentDirectory, startupInfo);
		}

		public IProcessInformation CreateWithLogin(string username, string domain, string password, ProcessLogonFlags logonFlags, string applicationName, string commandLine, ProcessCreationFlags creationFlags, EnvironmentBlockHandle environment, string currentDirectory, ProcessStartInfo startupInfo)
		{
			return ProcessHandle.CreateWithLogin(username, domain, password, logonFlags, applicationName, commandLine, creationFlags, environment, currentDirectory, startupInfo);
		}

		public IProcessInformation CreateWithToken(IToken token, ProcessLogonFlags logonFlags, string applicationName, string commandLine, ProcessCreationFlags creationFlags, IEnvironmentBlock environment, string currentDirectory, ProcessStartInfo startupInfo)
		{
			return ProcessHandle.CreateWithToken(token.Handle, logonFlags, applicationName, commandLine, creationFlags, environment.Handle, currentDirectory, startupInfo);
		}

		public IProcessInformation CreateWithToken(TokenHandle token, ProcessLogonFlags logonFlags, string applicationName, string commandLine, ProcessCreationFlags creationFlags, EnvironmentBlockHandle environment, string currentDirectory, ProcessStartInfo startupInfo)
		{
			return ProcessHandle.CreateWithToken(token, logonFlags, applicationName, commandLine, creationFlags, environment, currentDirectory, startupInfo);
		}

		public IProcess OpenProcess(int processId, ProcessAccessRights desiredAccess = ProcessAccessRights.AllAccess, bool inheritHandle = false)
		{
			var handle = ProcessHandle.OpenProcess(processId, desiredAccess, inheritHandle);
			return new Process(handle);
		}
	}
}
