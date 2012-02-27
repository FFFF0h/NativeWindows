using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace NativeWindows.IO
{
	public sealed class FileHandle : SafeHandle
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
			public static extern bool CreatePipe(out FileHandle readPipeHandle, out FileHandle writePipeHandle, SecurityAttributes pipeAttributes, int size);
		}

		public FileHandle()
			: base(IntPtr.Zero, true)
		{
		}

		private FileHandle(IntPtr invalidHandleValue, bool ownsHandle)
			: base(invalidHandleValue, ownsHandle)
		{
		}

		public static FileHandle GetStandardInput()
		{
			IntPtr standardInput = NativeMethods.GetStdHandle(StandardHandleType.Input);
			return new FileHandle(standardInput, false);
		}

		public static FileHandle GetStandardError()
		{
			IntPtr standardError = NativeMethods.GetStdHandle(StandardHandleType.Error);
			return new FileHandle(standardError, false);
		}

		public static FileHandle GetStandardOutput()
		{
			IntPtr standardOutput = NativeMethods.GetStdHandle(StandardHandleType.Output);
			return new FileHandle(standardOutput, false);
		}

		public static void CreatePipe(out FileHandle readPipeHandle, out FileHandle writePipeHandle)
		{
			using (var securityAttributes = new SecurityAttributes())
			{
				if (!NativeMethods.CreatePipe(out readPipeHandle, out writePipeHandle, securityAttributes, 0))
				{
					throw new Win32Exception();
				}
			}
		}

		protected override bool ReleaseHandle()
		{
			return handle.CloseHandle();
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
