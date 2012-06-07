namespace NativeWindows.Identity
{
	public interface ILsaPolicyFactory
	{
		ILsaPolicy Open(LsaAccessPolicy accessPolicy);
	}
}
