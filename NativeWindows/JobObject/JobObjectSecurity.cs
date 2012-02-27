using System;
using System.Security.AccessControl;
using System.Security.Principal;

namespace NativeWindows.JobObject
{
	public class JobObjectSecurity : CommonObjectSecurity
	{
		public JobObjectSecurity(bool isContainer) : base(isContainer)
		{
		}

		public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType accessControlType)
		{
			return new JobObjectAccessRule(identityReference, (JobObjectAccessRights)accessMask, accessControlType);
		}

		public void AddAccessRule(IdentityReference identityReference, JobObjectAccessRights accessMask, AccessControlType accessControlType)
		{
			AddAccessRule(new JobObjectAccessRule(identityReference, accessMask, accessControlType));
		}

		public override Type AccessRightType
		{
			get
			{
				return typeof(JobObjectAccessRights);
			}
		}

		public override Type AccessRuleType
		{
			get
			{
				return typeof(JobObjectAccessRule);
			}
		}

		public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		{
			throw new NotImplementedException();
		}

		public override Type AuditRuleType
		{
			get
			{
				throw new NotImplementedException();
			}
		}
	}
}
