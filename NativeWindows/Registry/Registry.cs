using System;
using System.Runtime.InteropServices;

namespace NativeWindows.Registry
{
	public class Registry
	{
		private static class NativeMethods
		{
			[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern SystemErrorCode RegQueryValueExW(UIntPtr key, string valueName, IntPtr reserved, IntPtr type, IntPtr data, ref uint dataLength);
		}

		public static MarshalHGlobal ReqQueryValueEx(UIntPtr hkey, string valueName, uint initialDataSize = 512 * 1024, uint bufferIncrecementSize = 64 * 1024)
		{
			uint dataLength = 0;
			SystemErrorCode status = NativeMethods.RegQueryValueExW(hkey, valueName, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, ref dataLength);
			if (status == SystemErrorCode.ErrorMoreData)
			{
				dataLength = initialDataSize;
			}
			else if (status != SystemErrorCode.ErrorSuccess)
			{
				throw ErrorHelper.GetWin32Exception(status);
			}

			var data = new MarshalHGlobal(dataLength);
			try
			{
				// In some cases the data may have grown since the initial datasize query, so we will have to resize until we can contain all the data.
				while ((status = NativeMethods.RegQueryValueExW(hkey, valueName, IntPtr.Zero, IntPtr.Zero, data.Pointer, ref dataLength)) == SystemErrorCode.ErrorMoreData)
				{
					dataLength += bufferIncrecementSize;
					data.Dispose();
					data = new MarshalHGlobal(dataLength);
				}

				if (status != SystemErrorCode.ErrorSuccess)
				{
					throw ErrorHelper.GetWin32Exception(status);
				}

				return data;
			}
			catch
			{
				data.Dispose();
				throw;
			}
		}

		//private static byte[] ToArray(IntPtr dataPtr, uint dataLength)
		//{
		//  // Why marshal cannot copy values over 2GB is so insane stupid, one more example of half implemented MS APIs
		//  if (dataLength > int.MaxValue)
		//  {
		//    throw new NotSupportedException();
		//  }

		//  var data = new byte[dataLength];
		//  Marshal.Copy(dataPtr, data, 0, data.Length);
		//  return data;
		//}
	}
}
