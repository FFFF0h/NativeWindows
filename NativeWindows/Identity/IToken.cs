using System;
using System.IO;
using NativeWindows.Security;

namespace NativeWindows.Identity
{
	public interface IToken : IDisposable
	{
		TokenHandle Handle { get; }

		IToken DuplicateTokenEx(TokenAccessRights desiredAccess, SecurityImpersonationLevel impersonationLevel, TokenType tokenType);
		void LoadUserProfile(ref ProfileInfo profileInfo);
		void UnloadUserProfile(ref ProfileInfo profileInfo);
		void DeleteUserProfile();
		DirectoryInfo GetUserProfileDirectory();
		SidAndAttributes GetUserTokenInformation();
		SidAndAttributes[] GetGroupsTokenInformation(TokenInformationClass tokenInformationClass);
	}
}
