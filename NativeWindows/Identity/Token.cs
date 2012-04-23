using System.IO;
using NativeWindows.Security;

namespace NativeWindows.Identity
{
	public class Token : IToken
	{
		private readonly TokenHandle _handle;

		public Token(TokenHandle handle)
		{
			_handle = handle;
		}

		public void Dispose()
		{
			_handle.Dispose();
		}

		public TokenHandle Handle
		{
			get
			{
				return _handle;
			}
		}

		public IToken DuplicateTokenEx(TokenAccessRights desiredAccess, SecurityImpersonationLevel impersonationLevel, TokenType tokenType)
		{
			var handle = _handle.DuplicateTokenEx(desiredAccess, impersonationLevel, tokenType);
			return new Token(handle);
		}

		public void LoadUserProfile(ref ProfileInfo profileInfo)
		{
			_handle.LoadUserProfile(ref profileInfo);
		}

		public void UnloadUserProfile(ref ProfileInfo profileInfo)
		{
			_handle.UnloadUserProfile(ref profileInfo);
		}

		public void DeleteUserProfile()
		{
			_handle.DeleteUserProfile();
		}

		public DirectoryInfo GetUserProfileDirectory()
		{
			return _handle.GetUserProfileDirectory();
		}

		public SidAndAttributes GetUserTokenInformation()
		{
			return _handle.GetUserTokenInformation();
		}

		public SidAndAttributes[] GetGroupsTokenInformation(TokenInformationClass tokenInformationClass)
		{
			return _handle.GetGroupsTokenInformation(tokenInformationClass);
		}
	}
}
