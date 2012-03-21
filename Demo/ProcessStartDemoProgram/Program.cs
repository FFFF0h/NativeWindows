using System;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Security;
using System.Security.AccessControl;
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

			using (var userHandle = UserHandle.Logon(username, ".", GetSecureString(password)))
			{
				using (var environmentBlockHandle = EnvironmentBlockHandle.Create(userHandle, false))
				{
					var processStartInfo = new ProcessStartInfo();

					ProcessInformation processInformation;
					processStartInfo.Desktop = string.Empty;
					string commandLine = string.Format("\"{0}\"", typeof(TestProgramWhileTrue.Program).Assembly.Location);
					var currentUser = WindowsIdentity.GetCurrent();
					var processUser = new WindowsIdentity(userHandle.DangerousGetHandle());

					var access = new ProcessSecurity();
					access.AddAccessRule(currentUser.Owner, ProcessAccessRights.AllAccess, AccessControlType.Allow);
					access.AddAccessRule(processUser.Owner, ProcessAccessRights.AllAccess, AccessControlType.Allow);

					if (Environment.UserInteractive)
					{
						// Running on a desktop (non-service) will require access the current user's windowstation and desktop
						using (var stationHandle = WindowStationHandle.GetProcessWindowStation())
						{
							var security = new WindowStationSecurity(stationHandle, AccessControlSections.Access);
							security.AddAccessRule(processUser.Owner, WindowStationAccessRights.AllAccess, AccessControlType.Allow);
							security.ApplyChangesTo(stationHandle);
						}

						using (var desktopHandle = DesktopHandle.GetFromCurrentThread())
						{
							var security = new DesktopSecurity(desktopHandle, AccessControlSections.Access);
							security.AddAccessRule(processUser.Owner, DesktopAccessRights.AllAccess, AccessControlType.Allow);
							security.ApplyChangesTo(desktopHandle);
						}
					}

					processInformation = ProcessHandle.CreateAsUser(userHandle, null, commandLine, false, ProcessCreationFlags.NewConsole | ProcessCreationFlags.UnicodeEnvironment, environmentBlockHandle, Environment.CurrentDirectory, processStartInfo, access);

					using (processInformation)
					{
						Process process = Process.GetProcessById(processInformation.ProcessId);
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
