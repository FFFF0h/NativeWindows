﻿using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace NativeWindows.User
{
	public sealed class EnvironmentBlockHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		private static class NativeMethods
		{
			[DllImport("userenv.dll", CharSet = CharSet.Auto, SetLastError = true)]
			public static extern bool CreateEnvironmentBlock(out EnvironmentBlockHandle lpEnvironment, IntPtr userToken, bool inherit);

			[DllImport("userenv.dll", SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool DestroyEnvironmentBlock(IntPtr environment);
		}

		public static EnvironmentBlockHandle Create(IntPtr userToken, bool inherit)
		{
			EnvironmentBlockHandle environmentBlockHandle;
			if (!NativeMethods.CreateEnvironmentBlock(out environmentBlockHandle, userToken, inherit))
			{
				throw new Win32Exception();
			}
			return environmentBlockHandle;
		}

		public EnvironmentBlockHandle()
			: base(true)
		{
		}

		protected override bool ReleaseHandle()
		{
			return NativeMethods.DestroyEnvironmentBlock(handle);
		}
	}
}