using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using NativeWindows.ProcessAndThread;

namespace NativeWindows.JobObject
{
	public sealed class JobObjectHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		private static class NativeMethods
		{
			[DllImport("kernel32.dll", SetLastError = true)]
			public static extern bool AssignProcessToJobObject(JobObjectHandle handle, IntPtr processHandle);

			[DllImport("kernel32.dll", SetLastError = true)]
			public static extern bool AssignProcessToJobObject(JobObjectHandle handle, ProcessHandle processHandle);

			[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
			public static extern JobObjectHandle CreateJobObject(SecurityAttributes jobAttributes, string name);

			[DllImport("kernel32.dll")]
			public static extern JobObjectHandle OpenJobObject(JobObjectAccessRights desiredAccess, bool inheritHandle, string name);

			[DllImport("kernel32.dll")]
			public static extern bool SetInformationJobObject(JobObjectHandle handle, JobObjectType jobObjectClass, IntPtr jobObjectInfo, uint jobObjectInfoLength);

			[DllImport("kernel32.dll")]
			public static extern bool QueryInformationJobObject(JobObjectHandle handle, JobObjectType jobObjectClass, IntPtr jobObjectInfo, uint jobObjectInfoLength, IntPtr returnLength);

			[DllImport("kernel32.dll")]
			public static extern bool TerminateJobObject(JobObjectHandle handle, uint exitCode);
		}

		public static JobObjectHandle Create(string name = null)
		{
			JobObjectHandle jobObjectHandle = NativeMethods.CreateJobObject(IntPtr.Zero, name);
			if (jobObjectHandle.IsInvalid)
			{
				throw new Win32Exception();
			}
			return jobObjectHandle;
		}

		public static JobObjectHandle Create(JobObjectSecurity security, string name = null)
		{
			using (var securityAttributes = new SecurityAttributes(security))
			{
				JobObjectHandle jobObjectHandle = NativeMethods.CreateJobObject(securityAttributes, name);
				if (jobObjectHandle.IsInvalid)
				{
					throw new Win32Exception();
				}
				return jobObjectHandle;
			}
		}

		public static JobObjectHandle Open(string name, bool inheritHandle = false, JobObjectAccessRights desiredAccess = JobObjectAccessRights.AllAccess)
		{
			JobObjectHandle jobObjectHandle = NativeMethods.OpenJobObject(desiredAccess, inheritHandle, name);
			if (jobObjectHandle.IsInvalid)
			{
				throw new Win32Exception();
			}
			return jobObjectHandle;
		}

		public JobObjectHandle()
			: base(true)
		{
		}

		/// <remarks>
		/// This will throw Access violation if UAC or PCA already has a job object attached to the process, so remember
		/// to disable those before attaching to a process.
		/// http://stackoverflow.com/questions/89588/assignprocesstojobobject-fails-with-access-denied-error-when-running-under-the
		/// </remarks>
		public void AssignProcess(Process process)
		{
			if (!NativeMethods.AssignProcessToJobObject(this, process.Handle))
			{
				throw new Win32Exception();
			}
		}

		public void AssignProcess(ProcessHandle processHandle)
		{
			if (!NativeMethods.AssignProcessToJobObject(this, processHandle))
			{
				throw new Win32Exception();
			}
		}

		public void SetInformation<T>(T jobObjectStructureWrapper) where T : IJobObjectSettable
		{
			JobObjectType jobObjectType = jobObjectStructureWrapper.JobType;
			int length = Marshal.SizeOf(jobObjectStructureWrapper.GetType());
			IntPtr structurePtr = Marshal.AllocHGlobal(length);
			try
			{
				Marshal.StructureToPtr(jobObjectStructureWrapper, structurePtr, false);
				if (!NativeMethods.SetInformationJobObject(this, jobObjectType, structurePtr, (uint)length))
				{
					throw new Win32Exception();
				}
			}
			finally
			{
				Marshal.FreeHGlobal(structurePtr);
			}
		}

		public T GetInformation<T>() where T : IJobObjectQueryable, new()
		{
			var instance = new T();
			GetInformation(instance);
			return instance;
		}

		public void GetInformation<T>(T jobObjectStructureWrapper) where T : IJobObjectQueryable
		{
			JobObjectType jobObjectType = jobObjectStructureWrapper.JobType;
			int length = Marshal.SizeOf(typeof(T));
			IntPtr structurePtr = Marshal.AllocHGlobal(length);
			try
			{
				if (!NativeMethods.QueryInformationJobObject(this, jobObjectType, structurePtr, (uint)length, IntPtr.Zero))
				{
					throw new Win32Exception();
				}
				Marshal.PtrToStructure(structurePtr, jobObjectStructureWrapper);
			}
			finally
			{
				Marshal.FreeHGlobal(structurePtr);
			}
		}

		public void Terminate(int exitCode)
		{
			if (!NativeMethods.TerminateJobObject(this, (uint)exitCode))
			{
				throw new Win32Exception();
			}
		}

		protected override bool ReleaseHandle()
		{
			return handle.CloseHandle();
		}
	}
}
