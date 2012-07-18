namespace NativeWindows.Identity
{
	public class NetUserInformation
	{
		public string Username
		{
			get;
			set;
		}

		public string Password
		{
			get;
			set;
		}

		public uint PasswordAge
		{
			get;
			set;
		}

		public NetUserPrivilige Privilege
		{
			get;
			set;
		}

		public string HomeDirectory
		{
			get;
			set;
		}

		public string Comment
		{
			get;
			set;
		}

		public NetUserFlags Flags
		{
			get;
			set;
		}

		public string ScriptPath
		{
			get;
			set;
		}
	}
}
