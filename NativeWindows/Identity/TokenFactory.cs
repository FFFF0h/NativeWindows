using System.Security;

namespace NativeWindows.Identity
{
	public class TokenFactory : ITokenFactory
	{
		public IToken Logon(string username, string domain, SecureString password, UserLogonType logonType = UserLogonType.Interactive, UserLogonProvider logonProvider = UserLogonProvider.Default)
		{
			var handle = TokenHandle.Logon(username, domain, password, logonType, logonProvider);
			return new Token(handle);
		}

		public IToken Logon(string username, string domain, string password, UserLogonType logonType = UserLogonType.Interactive, UserLogonProvider logonProvider = UserLogonProvider.Default)
		{
			var handle = TokenHandle.Logon(username, domain, password, logonType, logonProvider);
			return new Token(handle);
		}
	}
}
