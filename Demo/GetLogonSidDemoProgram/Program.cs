using System.Linq;
using System.Security.Principal;
using NativeWindows.User;

namespace GetLogonSidDemoProgram
{
	public class Program
	{
		public static void Main(string[] args)
		{
			using (var windowsIdentity = WindowsIdentity.GetCurrent())
			{
				using (var token = new UserHandle(windowsIdentity))
				{
					var groups = token.GetGroupsTokenInformation(TokenInformationClass.TokenLogonSid);
					SecurityIdentifier securityIdentifier = groups.Single().SecurityIdentifier;
					var isAccount = securityIdentifier.IsAccountSid();
					var text = securityIdentifier.ToString();
				}
			}
		}
	}
}
