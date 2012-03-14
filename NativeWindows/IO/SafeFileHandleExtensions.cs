using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Microsoft.Win32.SafeHandles;
using NativeWindows.ErrorHandling;
using NativeWindows.ProcessAndThread;

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

			[DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true, BestFitMapping = false)]
			[ResourceExposure(ResourceScope.Machine)]
			public static extern bool DuplicateHandle(ProcessHandle sourceProcessHandle, SafeHandle sourceHandle, ProcessHandle targetProcess, out SafeFileHandle targetHandle, FileAccessRights desiredAccess, bool inheritHandle, DuplicateHandleOptions options);
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

		public static SafeFileHandle DuplicateHandle(this SafeFileHandle sourceHandle, ProcessHandle sourceProcess, ProcessHandle targetProcess, FileAccessRights desiredAccess, bool inheritHandle, DuplicateHandleOptions options)
		{
			SafeFileHandle targetHandle;
			if (!NativeMethods.DuplicateHandle(sourceProcess, sourceHandle, targetProcess, out targetHandle, desiredAccess, inheritHandle, options))
			{
				ErrorHelper.ThrowCustomWin32Exception();
			}
			return targetHandle;
		}

		public static SafeFileHandle DuplicateHandle(this SafeFileHandle sourceHandle, ProcessHandle sourceAndTargetProcess, FileAccessRights desiredAccess, bool inheritHandle, DuplicateHandleOptions options)
		{
			SafeFileHandle targetHandle;
			if (!NativeMethods.DuplicateHandle(sourceAndTargetProcess, sourceHandle, sourceAndTargetProcess, out targetHandle, desiredAccess, inheritHandle, options))
			{
				ErrorHelper.ThrowCustomWin32Exception();
			}
			return targetHandle;
		}

		public static void CreatePipe(out SafeFileHandle readPipeHandle, out SafeFileHandle writePipeHandle)
		{
			using (var securityAttributes = new SecurityAttributes())
			{
				if (!NativeMethods.CreatePipe(out readPipeHandle, out writePipeHandle, securityAttributes, 0))
				{
					ErrorHelper.ThrowCustomWin32Exception();
				}
			}
		}
	}
}
