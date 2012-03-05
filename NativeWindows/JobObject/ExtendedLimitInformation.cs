using System;
using System.Runtime.InteropServices;

namespace NativeWindows.JobObject
{
	/// <summary>
	/// All the Nullable properties is optional fields, where null means that the field will not be used.
	/// </summary>
	/// <remarks>
	/// JOB_OBJECT_LIMIT_PRESERVE_JOB_TIME
	/// JOB_OBJECT_LIMIT_JOB_TIME
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public sealed class ExtendedLimitInformation : IJobObjectQueryable, IJobObjectSettable
	{
		// See: http://msdn.microsoft.com/en-us/library/windows/desktop/ms684147(v=vs.85).aspx
		[Flags]
		private enum JobObjectLimitFlags : uint
		{
			Workingset = 0x00000001,
			ProcessTime = 0x00000002,
			JobTime = 0x00000004,
			ActiveProcess = 0x00000008,
			Affinity = 0x00000010,
			PriorityClass = 0x00000020,
			PreserveJobTime = 0x00000040,
			SchedulingClass = 0x00000080,
			ProcessMemory = 0x00000100,
			JobMemory = 0x00000200,
			DieOnUnhandledException = 0x00000400,
			BreakawayOk = 0x00000800,
			SilentBreakawayOk = 0x00001000,
			KillOnJobClose = 0x00002000,
			SubsetAffinity = 0x00004000,
		}

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
		private struct BasicLimitInformation
		{
			public long PerProcessUserTimeLimit;
			public long PerJobUserTimeLimit;
			public JobObjectLimitFlags LimitFlags;
			public UIntPtr MinimumWorkingSetSize; // size_t
			public UIntPtr MaximumWorkingSetSize; // size_t
			public uint ActiveProcessLimit;
			public UIntPtr Affinity; // size_t
			public uint PriorityClass;
			public uint SchedulingClass;
		}

		private BasicLimitInformation _basicLimitInformation;
		private IoCounters _ioInfo; // Reserved!
		private UIntPtr _processMemoryLimit; // size_t
		private UIntPtr _jobMemoryLimit; // size_t
		private UIntPtr _peakProcessMemoryUsed; // size_t
		private UIntPtr _peakJobMemoryUsed; // size_t

		public ExtendedLimitInformation()
		{
			_basicLimitInformation.LimitFlags |= JobObjectLimitFlags.PreserveJobTime;
		}

		/// <summary>
		/// If the JobMemoryLimit is not null then, this member specifies the limit for the virtual memory that can be committed for the job. Otherwise, this member is ignored.
		/// </summary>
		public ulong? JobMemoryLimit
		{
			get
			{
				return GetWithFlag(() => _jobMemoryLimit.ToUInt64(), JobObjectLimitFlags.JobMemory);
			}
			set
			{
				SetWithFlag(x => _jobMemoryLimit = new UIntPtr(x), value, JobObjectLimitFlags.JobMemory);
			}
		}

		/// <summary>
		/// If the ProcessMemoryLimit is not null then, this member specifies the limit for the virtual memory that can be committed by a process. Otherwise, this member is ignored.
		/// </summary>
		public ulong? ProcessMemoryLimit
		{
			get
			{
				return GetWithFlag(() => _processMemoryLimit.ToUInt64(), JobObjectLimitFlags.ProcessMemory);
			}
			set
			{
				SetWithFlag(x => _processMemoryLimit = new UIntPtr(x), value, JobObjectLimitFlags.ProcessMemory);
			}
		}

		/// <summary>
		/// If the PerProcessUserTimeLimit is not null then, this member is the per-process user-mode execution time limit, in 100-nanosecond ticks. Otherwise, this member is ignored.
		/// The system periodically checks to determine whether each process associated with the job has accumulated more user-mode time than the set limit. If it has, the process is terminated.
		/// If the job is nested, the effective limit is the most restrictive limit in the job chain.
		/// </summary>
		public long? PerProcessUserTimeLimit
		{
			get
			{
				return GetWithFlag(() => _basicLimitInformation.PerProcessUserTimeLimit, JobObjectLimitFlags.ProcessTime);
			}
			set
			{
				SetWithFlag(x => _basicLimitInformation.PerProcessUserTimeLimit = x, value, JobObjectLimitFlags.ProcessTime);
			}
		}

		// TODO: Not complete JOBOBJECT_END_OF_JOB_TIME_INFORMATION has not been defined yet
		/// <summary>
		/// If the PerJobUserTimeLimit is not null then, this member is the per-job user-mode execution time limit, in 100-nanosecond ticks. Otherwise, this member is ignored.
		/// The system adds the current time of the processes associated with the job to this limit. For example, if you set this limit to 1 minute, and the job has a process that has accumulated 5 minutes of user-mode time, the limit actually enforced is 6 minutes.
		/// The system periodically checks to determine whether the sum of the user-mode execution time for all processes is greater than this end-of-job limit. If it is, the action specified in the EndOfJobTimeAction member of the JOBOBJECT_END_OF_JOB_TIME_INFORMATION structure is carried out. By default, all processes are terminated and the status code is set to ERROR_NOT_ENOUGH_QUOTA.
		/// To register for notification when this limit is exceeded without terminating processes, use the SetInformationJobObject function with the JobObjectNotificationLimitInformation information class.
		/// </summary>
		public long? PerJobUserTimeLimit
		{
			get
			{
				if (_basicLimitInformation.LimitFlags.HasFlag(JobObjectLimitFlags.JobTime))
				{
					return _basicLimitInformation.PerJobUserTimeLimit;
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					_basicLimitInformation.LimitFlags &= ~JobObjectLimitFlags.JobTime;
					_basicLimitInformation.LimitFlags |= JobObjectLimitFlags.PreserveJobTime;
				}
				else
				{
					_basicLimitInformation.LimitFlags |= JobObjectLimitFlags.JobTime;
					_basicLimitInformation.LimitFlags &= ~JobObjectLimitFlags.PreserveJobTime;
					_basicLimitInformation.PerJobUserTimeLimit = value.Value;
				}
			}
		}

		/// <summary>
		/// If the ActiveProcessLimit is not null then, this member is the active process limit for the job. Otherwise, this member is ignored.
		/// If you try to associate a process with a job, and this causes the active process count to exceed this limit, the process is terminated and the association fails.
		/// </summary>
		public uint? ActiveProcessLimit
		{
			get
			{
				return GetWithFlag(() => _basicLimitInformation.ActiveProcessLimit, JobObjectLimitFlags.ActiveProcess);
			}
			set
			{
				SetWithFlag(x => _basicLimitInformation.ActiveProcessLimit = x, value, JobObjectLimitFlags.ActiveProcess);
			}
		}

		/// <summary>
		/// If the MinimumWorkingSetSize or MaximumWorkingSetSize is not null then, this member is the minimum working set size for each process associated with the job. Otherwise, this member is ignored.
		/// If MaximumWorkingSetSize is nonzero / non-null, MinimumWorkingSetSize cannot be zero or null.
		/// </summary>
		public ulong? MinimumWorkingSetSize
		{
			get
			{
				return GetWithFlag(() => _basicLimitInformation.MinimumWorkingSetSize.ToUInt64(), JobObjectLimitFlags.Workingset);
			}
			set
			{
				SetWithFlag(x => _basicLimitInformation.MinimumWorkingSetSize = new UIntPtr(x), value, JobObjectLimitFlags.Workingset);
			}
		}

		/// <summary>
		/// If the MinimumWorkingSetSize or MaximumWorkingSetSize is not null then, this member is the maximum working set size for each process associated with the job. Otherwise, this member is ignored.
		/// If MinimumWorkingSetSize is nonzero, MaximumWorkingSetSize cannot be zero.
		/// </summary>
		public ulong? MaximumWorkingSetSize
		{
			get
			{
				return GetWithFlag(() => _basicLimitInformation.MaximumWorkingSetSize.ToUInt64(), JobObjectLimitFlags.Workingset);
			}
			set
			{
				SetWithFlag(x => _basicLimitInformation.MaximumWorkingSetSize = new UIntPtr(x), value, JobObjectLimitFlags.Workingset);
			}
		}

		/// <summary>
		/// The peak memory used by any process ever associated with the job.
		/// </summary>
		public ulong PeakProcessMemoryUsed
		{
			get
			{
				return _peakProcessMemoryUsed.ToUInt64();
			}
		}

		/// <summary>
		/// The peak memory usage of all processes currently associated with the job.
		/// </summary>
		public ulong PeakJobMemoryUsed
		{
			get
			{
				return _peakJobMemoryUsed.ToUInt64();
			}
		}

		/// <summary>
		/// Causes all processes associated with the job to terminate when the last handle to the job is closed.
		/// </summary>
		public bool KillOnJobClose
		{
			get
			{
				return _basicLimitInformation.LimitFlags.HasFlag(JobObjectLimitFlags.KillOnJobClose);
			}
			set
			{
				if (value)
				{
					_basicLimitInformation.LimitFlags |= JobObjectLimitFlags.KillOnJobClose;
				}
				else
				{
					_basicLimitInformation.LimitFlags &= ~JobObjectLimitFlags.KillOnJobClose;
				}
			}
		}

		// TODO: GetProcessAffinityMask
		/// <summary>
		/// If the Affinity is not null then, this member is the processor affinity for all processes associated with the job. Otherwise, this member is ignored.
		/// The affinity must be a subset of the system affinity mask obtained by calling the GetProcessAffinityMask function. The affinity of each thread is set to this value, but threads are free to subsequently set their affinity, as long as it is a subset of the specified affinity mask. Processes cannot set their own affinity mask.
		/// </summary>
		public ulong? Affinity
		{
			get
			{
				return GetWithFlag(() => _basicLimitInformation.Affinity.ToUInt64(), JobObjectLimitFlags.Affinity);
			}
			set
			{
				SetWithFlag(x => _basicLimitInformation.Affinity = new UIntPtr(x), value, JobObjectLimitFlags.Affinity);
			}
		}

		/// <summary>
		/// If the PriorityClass is not null then, this member is the priority class for all processes associated with the job. Otherwise, this member is ignored.
		/// Processes and threads cannot modify their priority class. The calling process must enable the SE_INC_BASE_PRIORITY_NAME privilege.
		/// </summary>
		public uint? PriorityClass
		{
			get
			{
				return GetWithFlag(() => _basicLimitInformation.PriorityClass, JobObjectLimitFlags.PriorityClass);
			}
			set
			{
				SetWithFlag(x => _basicLimitInformation.PriorityClass = x, value, JobObjectLimitFlags.PriorityClass);
			}
		}

		/// <summary>
		/// If the SchedulingClass is not null then, this member is the scheduling class for all processes associated with the job. Otherwise, this member is ignored.
		/// The valid values are 0 to 9. Use 0 for the least favorable scheduling class relative to other threads, and 9 for the most favorable scheduling class relative to other threads. By default, this value is 5. To use a scheduling class greater than 5, the calling process must enable the SE_INC_BASE_PRIORITY_NAME privilege.
		/// </summary>
		public uint? SchedulingClass
		{
			get
			{
				return GetWithFlag(() => _basicLimitInformation.SchedulingClass, JobObjectLimitFlags.SchedulingClass);
			}
			set
			{
				SetWithFlag(x => _basicLimitInformation.SchedulingClass = x, value, JobObjectLimitFlags.SchedulingClass);
			}
		}

		private T? GetWithFlag<T>(Func<T> getFunc, JobObjectLimitFlags flag) where T : struct
		{
			if (_basicLimitInformation.LimitFlags.HasFlag(flag))
			{
				return getFunc();
			}
			return null;
		}

		private void SetWithFlag<T>(Action<T> setAction, T? value, JobObjectLimitFlags flag) where T : struct
		{
			if (value == null)
			{
				_basicLimitInformation.LimitFlags &= ~flag;
			}
			else
			{
				_basicLimitInformation.LimitFlags |= flag;
				setAction(value.Value);
			}
		}

		public JobObjectType JobType
		{
			get
			{
				return JobObjectType.ExtendedLimitInformation;
			}
		}
	}
}
