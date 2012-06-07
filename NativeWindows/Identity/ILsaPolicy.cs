using System;

namespace NativeWindows.Identity
{
	public interface ILsaPolicy : IDisposable
	{
		void AddRights(string name, params string[] userRights);

		LsaPolicyHandle Handle
		{
			get;
		}
	}
}
