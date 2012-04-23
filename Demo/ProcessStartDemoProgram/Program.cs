using System;
using System.DirectoryServices.AccountManagement;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using NativeWindows.Identity;
using NativeWindows.Processes;

namespace ProcessStartDemoProgram
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var tokenFactory = new TokenFactory();
			var environmentBlockFactory = new EnvironmentBlockFactory();
			var processFactory = new ProcessFactory();

			const string username = "testuserfoo";
			const string password = "1234";

			GetOrCreateUser(username, password);

			using (var token = tokenFactory.Logon(username, ".", GetSecureString(password)))
			{
				using (var environmentBlockHandle = environmentBlockFactory.Create(token, false))
				{
					var profileInfo = new ProfileInfo
					{
						Size = Marshal.SizeOf(typeof(ProfileInfo)),
						Username = username,
						DefaultPath = null,
					};
					token.LoadUserProfile(ref profileInfo);

					IProcessInformation processInformation;
					var processStartInfo = new ProcessStartInfo
					{
						Desktop = string.Empty,
					};
					string commandLine = string.Format("\"{0}\"", typeof(TestProgramWhileTrue.Program).Assembly.Location);

					if (Environment.UserInteractive)
					{
						processInformation = processFactory.CreateWithLogin(username, "", password,
							ProcessLogonFlags.None,
							null,
							commandLine,
							ProcessCreationFlags.NewConsole | ProcessCreationFlags.UnicodeEnvironment,
							environmentBlockHandle,
							Environment.CurrentDirectory,
							processStartInfo);
					}
					else
					{
						processInformation = processFactory.CreateAsUser(token,
							null,
							commandLine,
							false,
							ProcessCreationFlags.NewConsole | ProcessCreationFlags.UnicodeEnvironment,
							environmentBlockHandle,
							Environment.CurrentDirectory,
							processStartInfo);
					}

					using (processInformation)
					{
						Console.WriteLine("Press any key to kill");
						Console.ReadKey(intercept: true);
						processInformation.Process.Terminate(0);

						token.UnloadUserProfile(ref profileInfo);
						token.DeleteUserProfile();
						DeleteUser(username);
					}
				}
			}
		}

		private static void GetOrCreateUser(string username, string password)
		{
			try
			{
				var account = new NTAccount(username);
				var sid = account.Translate(typeof(SecurityIdentifier));
			}
			catch (IdentityNotMappedException)
			{
				using (var principalContext = new PrincipalContext(ContextType.Machine))
				{
					using (var userPrincipal = new UserPrincipal(principalContext))
					{
						userPrincipal.Name = username;
						userPrincipal.SetPassword(password);
						userPrincipal.Save();
					}
				}
			}
		}

		private static void DeleteUser(string username)
		{
			using (var principalContext = new PrincipalContext(ContextType.Machine))
			{
				using (var userPrincipal = UserPrincipal.FindByIdentity(principalContext, username))
				{
					userPrincipal.Delete();
				}
			}
		}

		private static SecureString GetSecureString(string unsecurePassword)
		{
			var secureString = new SecureString();
			foreach (char c in unsecurePassword)
			{
				secureString.AppendChar(c);
			}
			return secureString;
		}
	}
}
