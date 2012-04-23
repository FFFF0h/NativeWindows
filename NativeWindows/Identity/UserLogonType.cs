namespace NativeWindows.Identity
{
	public enum UserLogonType : uint
	{
		Interactive = 2,
		Network = 3,
		Batch = 4,
		Service = 5,
		Unlock = 7,
		NetworkCleartext = 8,
		NewCredentials = 9,
	}
}
