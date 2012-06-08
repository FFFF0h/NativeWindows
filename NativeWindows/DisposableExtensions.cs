using System;
using System.Collections.Generic;

namespace NativeWindows
{
	internal static class DisposableExtensions
	{
		public static void DisposeAll<T>(this IEnumerable<T> disposables)
			where T : IDisposable
		{
			foreach (var disposable in disposables)
			{
				disposable.Dispose();
			}
		}
	}
}
