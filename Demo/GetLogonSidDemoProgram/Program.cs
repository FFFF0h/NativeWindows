using System;
using System.Linq;
using System.Security.Principal;
using NativeWindows.Identity;

namespace GetLogonSidDemoProgram
{
	public class Program
	{
		public static void Main(string[] args)
		{
			using (var windowsIdentity = WindowsIdentity.GetCurrent())
			{
				using (var token = new TokenHandle(windowsIdentity))
				{
					var groups = token.GetGroupsTokenInformation(TokenInformationClass.TokenLogonSid);
					SecurityIdentifier securityIdentifier = groups.Single().SecurityIdentifier;
					Console.WriteLine("Is Account: {0}", securityIdentifier.IsAccountSid());
					Console.WriteLine("SID: {0}", securityIdentifier);
					Console.WriteLine("ProfilePath: {0}", token.GetUserProfileDirectory().FullName);
				}
			}
		}
	}
}
