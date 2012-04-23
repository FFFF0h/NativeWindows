using System;
using System.Collections.Generic;

namespace NativeWindows.Processes
{
	public interface IEnvironmentBlock : IDisposable
	{
		EnvironmentBlockHandle Handle { get; }

		IDictionary<string, string> GetEnvironmentVariables();
	}
}
