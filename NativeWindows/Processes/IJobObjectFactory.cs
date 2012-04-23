namespace NativeWindows.Processes
{
	public interface IJobObjectFactory
	{
		IJobObject Create(string name = null);
		IJobObject Create(JobObjectSecurity jobObjectSecurity, string name = null);
		IJobObject Open(string name, bool inheritHandle = false, JobObjectAccessRights desiredAccess = JobObjectAccessRights.AllAccess);
	}
}
