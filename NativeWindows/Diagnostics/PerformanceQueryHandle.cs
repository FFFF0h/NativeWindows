using System;
using System.Runtime.InteropServices;

namespace NativeWindows.Diagnostics
{
	public class PerformanceQueryHandle : SafeHandle
	{
		private static class NativeMethods
		{
			//PDH_STATUS PdhOpenQuery(__in   LPCTSTR szDataSource,__in   DWORD_PTR dwUserData,__out  PDH_HQUERY *phQuery);
			[DllImport("Pdh.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern int PdhOpenQuery(string dataSource, IntPtr userData, out PerformanceQueryHandle handle);

			//PDH_STATUS PdhCloseQuery(__in  PDH_HQUERY hQuery);
			[DllImport("Pdh.dll", SetLastError = true)]
			public static extern int PdhCloseQuery(IntPtr handle);

			//PDH_STATUS PdhAddCounter(__in   PDH_HQUERY hQuery,__in   LPCTSTR szFullCounterPath,__in   DWORD_PTR dwUserData,__out  PDH_HCOUNTER *phCounter);
			[DllImport("Pdh.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern int PdhAddCounter(PerformanceQueryHandle handle, string fullCounterPath, IntPtr userData, out PerformanceCounterHandle counterHandle);

			//PDH_STATUS PdhAddEnglishCounter(__in   PDH_HQUERY hQuery,__in   LPCTSTR szFullCounterPath,__in   DWORD_PTR dwUserData,__out  PDH_HCOUNTER *phCounter);
			[DllImport("Pdh.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			public static extern int PdhAddEnglishCounter(PerformanceQueryHandle handle, string fullCounterPath, IntPtr userData, out PerformanceCounterHandle counterHandle);

			//PDH_STATUS PdhCollectQueryData(__inout  PDH_HQUERY hQuery);
			[DllImport("Pdh.dll", SetLastError = true)]
			public static extern int PdhCollectQueryData(PerformanceQueryHandle handle);

			//PDH_STATUS PdhCollectQueryDataEx(__in  PDH_HQUERY hQuery,__in  DWORD dwIntervalTime,__in  HANDLE hNewDataEvent);
			//[DllImport("Pdh.dll", SetLastError = true)] // async with callback
			//public static extern int PdhCollectQueryDataEx(PerformanceQueryHandle handle, int intervalTime, SafeWaitHandle newDataEvent);
		}

		public static PerformanceQueryHandle Open(string dataSource = null)
		{
			PerformanceQueryHandle handle;
			int errorCode = NativeMethods.PdhOpenQuery(dataSource, IntPtr.Zero, out handle);
			if (errorCode != (int)SystemErrorCode.ErrorSuccess)
			{
				throw ErrorHelper.GetPdhException(errorCode);
			}
			return handle;
		}

		public PerformanceQueryHandle() : base(IntPtr.Zero, true)
		{
		}

		public PerformanceQueryHandle(IntPtr handle, bool ownsHandle = true) : base(handle, ownsHandle)
		{
		}

		public void Collect()
		{
			int errorCode = NativeMethods.PdhCollectQueryData(this);
			if (errorCode != (int)SystemErrorCode.ErrorSuccess)
			{
				throw ErrorHelper.GetPdhException(errorCode);
			}
		}

		public PerformanceCounterHandle AddPerformanceCounter(string fullCounterPath)
		{
			PerformanceCounterHandle counterHandle;
			int errorCode = NativeMethods.PdhAddCounter(this, fullCounterPath, IntPtr.Zero, out counterHandle);
			if (errorCode != (int)SystemErrorCode.ErrorSuccess)
			{
				throw ErrorHelper.GetPdhException(errorCode);
			}
			return counterHandle;
		}

		public PerformanceCounterHandle AddEnglishPerformanceCounter(string fullCounterPath)
		{
			PerformanceCounterHandle counterHandle;
			int errorCode = NativeMethods.PdhAddEnglishCounter(this, fullCounterPath, IntPtr.Zero, out counterHandle);
			if (errorCode != (int)SystemErrorCode.ErrorSuccess)
			{
				throw ErrorHelper.GetPdhException(errorCode);
			}
			return counterHandle;
		}

		protected override bool ReleaseHandle()
		{
			return NativeMethods.PdhCloseQuery(handle) == (int)SystemErrorCode.ErrorSuccess;
		}

		public override bool IsInvalid
		{
			get
			{
				return handle == IntPtr.Zero;
			}
		}
	}
}
