namespace NativeWindows.Processes
{
	public class JobObjectFactory : IJobObjectFactory
	{
		public IJobObject Create(string name = null)
		{
			var handle = JobObjectHandle.Create(name);
			return new JobObject(handle);
		}

		public IJobObject Create(JobObjectSecurity jobObjectSecurity, string name = null)
		{
			var handle = JobObjectHandle.Create(jobObjectSecurity, name);
			return new JobObject(handle);
		}

		public IJobObject Open(string name, bool inheritHandle = false, JobObjectAccessRights desiredAccess = JobObjectAccessRights.AllAccess)
		{
			var handle = JobObjectHandle.Open(name, inheritHandle, desiredAccess);
			return new JobObject(handle);
		}
	}
}
