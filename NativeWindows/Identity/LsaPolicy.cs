namespace NativeWindows.Identity
{
	public class LsaPolicy : ILsaPolicy
	{
		private readonly LsaPolicyHandle _handle;

		public LsaPolicy(LsaPolicyHandle handle)
		{
			_handle = handle;
		}

		public LsaPolicyHandle Handle
		{
			get
			{
				return _handle;
			}
		}

		public void Dispose()
		{
			_handle.Dispose();
		}

		public void AddRights(string name, params string[] userRights)
		{
			_handle.AddRights(name, userRights);
		}
	}
}
