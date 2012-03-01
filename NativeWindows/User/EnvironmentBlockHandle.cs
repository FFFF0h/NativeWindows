using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

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

		public static EnvironmentBlockHandle Create(IDictionary<string, string> environmentVariables)
		{
			var memoryStream = new MemoryStream();
			var streamWriter = new StreamWriter(memoryStream, Encoding.Unicode);

			// Destroy the unicode byte order marker
			streamWriter.Flush();
			memoryStream.Seek(0, SeekOrigin.Begin);

			foreach (var variable in environmentVariables)
			{
				streamWriter.Write("{0}={1}", variable.Key, variable.Value);
				streamWriter.Write((char)0);
			}

			streamWriter.Write((char)0);
			streamWriter.Flush();

			byte[] environmentByteData = memoryStream.ToArray();

			IntPtr data = Marshal.AllocHGlobal(environmentByteData.Length);
			Marshal.Copy(environmentByteData, 0, data, environmentByteData.Length);
			return new EnvironmentBlockHandle(data, true);
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

		private readonly bool _newEnvironmentBlock;

		public EnvironmentBlockHandle()
			: base(IntPtr.Zero, true)
		{
			_newEnvironmentBlock = false;
		}

		private EnvironmentBlockHandle(IntPtr handle, bool newEnvironmentBlock)
			: base(handle, true)
		{
			_newEnvironmentBlock = newEnvironmentBlock;
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
			if (_newEnvironmentBlock)
			{
				if (handle != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(handle);
					handle = IntPtr.Zero;
					return true;
				}
				return false;
			}
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
