namespace NativeWindows.Identity
{
	public interface INetLocalGroup
	{
		void Add(string groupName, string description, string serverName = null);
		void AddMember(string username, string groupName, string domainName = null, string serverName = null);
		bool Exists(string groupName, string serverName = null);
	}
}
