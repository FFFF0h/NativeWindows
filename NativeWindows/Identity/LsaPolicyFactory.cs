namespace NativeWindows.Identity
{
	public class LsaPolicyFactory : ILsaPolicyFactory
	{
		public ILsaPolicy Open(LsaAccessPolicy accessPolicy)
		{
			return new LsaPolicy(LsaPolicyHandle.Open(accessPolicy));
		}
	}
}
