using System;

namespace NativeWindows.Processes
{
	public class ProcessTimes
	{
		public DateTime CreationTime
		{
			get;
			set;
		}

		public DateTime? ExitTime
		{
			get;
			set;
		}

		public TimeSpan KernelTime
		{
			get;
			set;
		}

		public TimeSpan UserTime
		{
			get;
			set;
		}
	}
}
