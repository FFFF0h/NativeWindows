using NativeWindows.Identity;
using NativeWindows.Threads;

namespace NativeWindows.Processes
{
	public interface IProcessFactory
	{
		IProcessInformation Create(string applicationName, string commandLine, bool inheritHandles, ProcessCreationFlags creationFlags, IEnvironmentBlock environment, string currentDirectory, ProcessStartInfo startInfo, ProcessSecurity processSecurity = null, ThreadSecurity threadSecurity = null);
		IProcessInformation Create(string applicationName, string commandLine, bool inheritHandles, ProcessCreationFlags creationFlags, EnvironmentBlockHandle environment, string currentDirectory, ProcessStartInfo startInfo, ProcessSecurity processSecurity = null, ThreadSecurity threadSecurity = null);
		IProcessInformation CreateAsUser(IToken token, string applicationName, string commandLine, bool inheritHandles, ProcessCreationFlags creationFlags, IEnvironmentBlock environment, string currentDirectory, ProcessStartInfo startInfo, ProcessSecurity processSecurity = null, ThreadSecurity threadSecurity = null);
		IProcessInformation CreateAsUser(TokenHandle token, string applicationName, string commandLine, bool inheritHandles, ProcessCreationFlags creationFlags, EnvironmentBlockHandle environment, string currentDirectory, ProcessStartInfo startInfo, ProcessSecurity processSecurity = null, ThreadSecurity threadSecurity = null);
		IProcessInformation CreateWithLogin(string username, string domain, string password, ProcessLogonFlags logonFlags, string applicationName, string commandLine, ProcessCreationFlags creationFlags, IEnvironmentBlock environment, string currentDirectory, ProcessStartInfo startupInfo);
		IProcessInformation CreateWithLogin(string username, string domain, string password, ProcessLogonFlags logonFlags, string applicationName, string commandLine, ProcessCreationFlags creationFlags, EnvironmentBlockHandle environment, string currentDirectory, ProcessStartInfo startupInfo);
		IProcessInformation CreateWithToken(IToken token, ProcessLogonFlags logonFlags, string applicationName, string commandLine, ProcessCreationFlags creationFlags, IEnvironmentBlock environment, string currentDirectory, ProcessStartInfo startupInfo);
		IProcessInformation CreateWithToken(TokenHandle token, ProcessLogonFlags logonFlags, string applicationName, string commandLine, ProcessCreationFlags creationFlags, EnvironmentBlockHandle environment, string currentDirectory, ProcessStartInfo startupInfo);
		IProcess OpenProcess(int processId, ProcessAccessRights desiredAccess = ProcessAccessRights.AllAccess, bool inheritHandle = false);
	}
}
