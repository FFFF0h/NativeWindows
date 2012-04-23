using System.Collections.Generic;
using NativeWindows.Identity;

namespace NativeWindows.Processes
{
	public interface IEnvironmentBlockFactory
	{
		IEnvironmentBlock Create(IToken token, IDictionary<string, string> extraEnvironmentVariables);
		IEnvironmentBlock Create(TokenHandle token, IDictionary<string, string> extraEnvironmentVariables);
		IEnvironmentBlock Create(IDictionary<string, string> environmentVariables);
		IEnvironmentBlock Create(IToken token, bool inherit);
		IEnvironmentBlock Create(TokenHandle token, bool inherit);
	}
}
