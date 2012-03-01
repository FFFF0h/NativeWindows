using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace NativeWindows.User
{
	// For information about the environment block layout see:
	// http://msdn.microsoft.com/en-us/library/windows/desktop/ms682653(v=vs.85).aspx
	public sealed class EnvironmentBlockHandle : SafeHandle
	{
		private static class NativeMethods
		{
			[DllImport("userenv.dll", CharSet = CharSet.Auto, SetLastError = true)]
			public static extern bool CreateEnvironmentBlock(out EnvironmentBlockHandle lpEnvironment, UserHandle userHandle, bool inherit);

			[DllImport("userenv.dll", SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool DestroyEnvironmentBlock(IntPtr environment);
		}

		public static EnvironmentBlockHandle Create(UserHandle userHandle, bool inherit)
		{
			EnvironmentBlockHandle environmentBlockHandle;
			if (!NativeMethods.CreateEnvironmentBlock(out environmentBlockHandle, userHandle, inherit))
			{
				throw new Win32Exception();
			}
			return environmentBlockHandle;
		}

		public EnvironmentBlockHandle()
			: base(IntPtr.Zero, true)
		{
		}

		public unsafe IDictionary<string, string> GetEnvironmentVariables()
		{
			if (IsInvalid)
			{
				throw new InvalidOperationException("Cannot get envronmentvariable on an Invalid EnvironmentBlockHandle");
			}

			Environment.GetEnvironmentVariables();
			var dictionary = new Dictionary<string, string>();
			var builder = new StringBuilder();
			var pointer = (char*)handle.ToPointer();
			while (true)
			{
				char c = *pointer;
				if (c == (char)0)
				{
					if (builder.Length == 0)
					{
						break;
					}
					string[] tokens = builder.ToString().Split(new[] { '=' }, 2, StringSplitOptions.None);
					dictionary[tokens[0]] = tokens[1];
					builder = new StringBuilder();
				}
				else
				{
					builder.Append(c);
				}
				pointer++;
			}
			return dictionary;
		}

		protected override bool ReleaseHandle()
		{
			return NativeMethods.DestroyEnvironmentBlock(handle);
		}

		public override bool IsInvalid
		{
			get
			{
				return handle == IntPtr.Zero || handle == new IntPtr(-1);
			}
		}
	}
}
