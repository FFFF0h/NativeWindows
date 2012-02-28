using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace NativeWindows.IO
{
	public static class SafeFileHandleExtensions
	{
		private enum StandardHandleType
		{
			Input = -10,
			Output = -11,
			Error = -12,
		}

		private static class NativeMethods
		{
			[DllImport("kernel32.dll", SetLastError = true)]
			public static extern IntPtr GetStdHandle(StandardHandleType stdHandleType);

			[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			public static extern bool CreatePipe(out SafeFileHandle readPipeHandle, out SafeFileHandle writePipeHandle, SecurityAttributes pipeAttributes, int size);
		}

		public static SafeFileHandle GetStandardInput()
		{
			IntPtr standardInput = NativeMethods.GetStdHandle(StandardHandleType.Input);
			return new SafeFileHandle(standardInput, false);
		}

		public static SafeFileHandle GetStandardError()
		{
			IntPtr standardError = NativeMethods.GetStdHandle(StandardHandleType.Error);
			return new SafeFileHandle(standardError, false);
		}

		public static SafeFileHandle GetStandardOutput()
		{
			IntPtr standardOutput = NativeMethods.GetStdHandle(StandardHandleType.Output);
			return new SafeFileHandle(standardOutput, false);
		}

		public static void CreatePipe(out SafeFileHandle readPipeHandle, out SafeFileHandle writePipeHandle)
		{
			using (var securityAttributes = new SecurityAttributes())
			{
				if (!NativeMethods.CreatePipe(out readPipeHandle, out writePipeHandle, securityAttributes, 0))
				{
					throw new Win32Exception();
				}
			}
		}
	}
}
