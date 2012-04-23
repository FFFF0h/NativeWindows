using System.Runtime.InteropServices;

namespace NativeWindows.Processes
{
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BasicAndIoAccountingInformation : IJobObjectQueryable
	{
		[StructLayout(LayoutKind.Sequential)]
		private struct IoCounters
		{
			public ulong ReadOperationCount;
			public ulong WriteOperationCount;
			public ulong OtherOperationCount;
			public ulong ReadTransferCount;
			public ulong WriteTransferCount;
			public ulong OtherTransferCount;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct BasicAccountingInformation
		{
			public long TotalUserTime;
			public long TotalKernelTime;
			public long ThisPeriodTotalUserTime;
			public long ThisPeriodTotalKernelTime;
			public uint TotalPageFaultCount;
			public uint TotalProcesses;
			public uint ActiveProcesses;
			public uint TotalTerminatedProcesses;
		}

		private BasicAccountingInformation _basicAccountingInformation;
		private IoCounters _ioCounters;

		public long TotalUserTime
		{
			get
			{
				return _basicAccountingInformation.TotalUserTime;
			}
		}

		public long TotalKernelTime
		{
			get
			{
				return _basicAccountingInformation.TotalKernelTime;
			}
		}

		public long ThisPeriodTotalUserTime
		{
			get
			{
				return _basicAccountingInformation.ThisPeriodTotalUserTime;
			}
		}

		public long ThisPeriodTotalKernelTime
		{
			get
			{
				return _basicAccountingInformation.ThisPeriodTotalKernelTime;
			}
		}

		public uint TotalPageFaultCount
		{
			get
			{
				return _basicAccountingInformation.TotalPageFaultCount;
			}
		}

		public uint TotalProcesses
		{
			get
			{
				return _basicAccountingInformation.TotalProcesses;
			}
		}

		public uint ActiveProcesses
		{
			get
			{
				return _basicAccountingInformation.ActiveProcesses;
			}
		}

		public uint TotalTerminatedProcesses
		{
			get
			{
				return _basicAccountingInformation.TotalTerminatedProcesses;
			}
		}

		public ulong ReadOperationCount
		{
			get
			{
				return _ioCounters.ReadOperationCount;
			}
		}

		public ulong WriteOperationCount
		{
			get
			{
				return _ioCounters.WriteOperationCount;
			}
		}

		public ulong OtherOperationCount
		{
			get
			{
				return _ioCounters.OtherOperationCount;
			}
		}

		public ulong ReadTransferCount
		{
			get
			{
				return _ioCounters.ReadTransferCount;
			}
		}

		public ulong WriteTransferCount
		{
			get
			{
				return _ioCounters.WriteTransferCount;
			}
		}

		public ulong OtherTransferCount
		{
			get
			{
				return _ioCounters.OtherTransferCount;
			}
		}

		public JobObjectType JobType
		{
			get
			{
				return JobObjectType.BasicAndIoAccountingInformation;
			}
		}
	}
}
