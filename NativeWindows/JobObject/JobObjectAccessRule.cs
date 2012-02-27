using System.Security.AccessControl;
using System.Security.Principal;

namespace NativeWindows.JobObject
{
	public class JobObjectAccessRule : AccessRule
	{
		public JobObjectAccessRule(IdentityReference identity, JobObjectAccessRights accessMask, AccessControlType accessType)
			: base(identity, (int)accessMask, false, InheritanceFlags.None, PropagationFlags.None, accessType)
		{
		}

		public int AccessRights
		{
			get
			{
				return AccessMask;
			}
		}
	}
}
