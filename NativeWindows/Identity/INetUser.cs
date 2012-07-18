namespace NativeWindows.Identity
{
	public interface INetUser
	{
		void Add(NetUserInformation userInformation, string serverName = null);
		bool Exists(string username, string serverName = null);
		NetUserInformation GetInformation(string username, string serverName = null);
		void SetInformation(NetUserInformation information, string serverName = null);
	}
}
