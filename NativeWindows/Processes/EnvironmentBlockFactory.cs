using System.Collections.Generic;
using NativeWindows.Identity;

namespace NativeWindows.Processes
{
	public class EnvironmentBlockFactory : IEnvironmentBlockFactory
	{
		internal class EnvironmentBlock : IEnvironmentBlock
		{
			private readonly EnvironmentBlockHandle _handle;

			public EnvironmentBlock(EnvironmentBlockHandle handle)
			{
				_handle = handle;
			}

			public void Dispose()
			{
				_handle.Dispose();
			}

			public EnvironmentBlockHandle Handle
			{
				get
				{
					return _handle;
				}
			}

			public IDictionary<string, string> GetEnvironmentVariables()
			{
				return _handle.GetEnvironmentVariables();
			}
		}

		public IEnvironmentBlock Create(IToken token, IDictionary<string, string> extraEnvironmentVariables)
		{
			return Create(token.Handle, extraEnvironmentVariables);
		}

		public IEnvironmentBlock Create(TokenHandle token, IDictionary<string, string> extraEnvironmentVariables)
		{
			var handle = EnvironmentBlockHandle.Create(token, extraEnvironmentVariables);
			return new EnvironmentBlock(handle);
		}

		public IEnvironmentBlock Create(IDictionary<string, string> environmentVariables)
		{
			var handle = EnvironmentBlockHandle.Create(environmentVariables);
			return new EnvironmentBlock(handle);
		}

		public IEnvironmentBlock Create(IToken token, bool inherit)
		{
			return Create(token.Handle, inherit);
		}

		public IEnvironmentBlock Create(TokenHandle token, bool inherit)
		{
			var handle = EnvironmentBlockHandle.Create(token, inherit);
			return new EnvironmentBlock(handle);
		}
	}
}
