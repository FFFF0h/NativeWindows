using System.Security;

namespace NativeWindows.Identity
{
	public interface ITokenFactory
	{
		IToken Logon(string username, string domain, SecureString password, UserLogonType logonType = UserLogonType.Interactive, UserLogonProvider logonProvider = UserLogonProvider.Default);
		IToken Logon(string username, string domain, string password, UserLogonType logonType = UserLogonType.Interactive, UserLogonProvider logonProvider = UserLogonProvider.Default);
	}
}
