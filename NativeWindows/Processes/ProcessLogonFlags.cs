namespace NativeWindows.Processes
{
	public enum ProcessLogonFlags : uint
	{
		None = 0,

		LogonWithProfile = 0x00000001,
		LogonNetcredentialsOnly = 0x00000002,
	}
}
