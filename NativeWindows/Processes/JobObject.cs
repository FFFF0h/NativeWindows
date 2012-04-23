using System;

namespace NativeWindows.Processes
{
	public class JobObject : IJobObject
	{
		private readonly JobObjectHandle _handle;

		public JobObject(JobObjectHandle handle)
		{
			if (handle.IsInvalid || handle.IsClosed)
			{
				throw new ArgumentException("Invalid handle", "handle");
			}
			_handle = handle;
		}

		public void Dispose()
		{
			_handle.Dispose();
		}

		public JobObjectHandle Handle
		{
			get
			{
				return _handle;
			}
		}

		public void Terminate(int exitCode)
		{
			_handle.Terminate(exitCode);

		}

		public void AssignProcess(global::System.Diagnostics.Process process)
		{
			_handle.AssignProcess(process);
		}

		public void AssignProcess(IProcess process)
		{
			_handle.AssignProcess(process.Handle);
		}

		public void AssignProcess(ProcessHandle processHandle)
		{
			_handle.AssignProcess(processHandle);
		}

		public T GetInformation<T>() where T : IJobObjectQueryable, new()
		{
			return _handle.GetInformation<T>();
		}

		public void SetInformation<T>(T jobObjectStructureWrapper) where T : IJobObjectSettable
		{
			_handle.SetInformation(jobObjectStructureWrapper);
		}
	}
}
