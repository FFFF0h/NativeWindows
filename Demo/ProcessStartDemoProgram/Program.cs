using System;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Security;
using System.Security.Principal;
using NativeWindows.ProcessAndThread;
using NativeWindows.User;
using ProcessStartInfo = NativeWindows.ProcessAndThread.ProcessStartInfo;

namespace ProcessStartDemoProgram
{
	public class Program
	{
		public static void Main(string[] args)
		{
			const string username = "testuserfoo";
			const string password = "1234";

			GetOrCreateUser(username, password);

			UserHandle primaryUserHandle;
			using (var userHandle = UserHandle.Logon(username, ".", GetSecureString(password)))
			{
				primaryUserHandle = userHandle.DuplicateTokenEx(TokenAccessRights.AllAccess, SecurityImpersonationLevel.SecurityImpersonation, TokenType.TokenPrimary);
			}

			using (primaryUserHandle)
			{
				using (var environmentBlockHandle = EnvironmentBlockHandle.Create(primaryUserHandle, false))
				{
					var processStartInfo = new ProcessStartInfo();
					string commandLine = string.Format("\"{0}\"", typeof(TestProgramWhileTrue.Program).Assembly.Location);
					using (var processInformation = ProcessHandle.CreateAsUser(primaryUserHandle, null, commandLine, false, ProcessCreationFlags.NewConsole | ProcessCreationFlags.Suspended | ProcessCreationFlags.UnicodeEnvironment, environmentBlockHandle, Environment.CurrentDirectory, processStartInfo))
					{
						Process process = Process.GetProcessById(processInformation.ProcessId);
						processInformation.ThreadHandle.Resume();
						Console.WriteLine("Press any key to kill");
						Console.ReadKey(intercept: true);
						process.Kill();

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
