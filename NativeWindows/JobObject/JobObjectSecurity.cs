using System;
using System.Security.AccessControl;
using System.Security.Principal;

namespace NativeWindows.JobObject
{
	public sealed class JobObjectAuditRule : AuditRule
	{
		public JobObjectAuditRule(IdentityReference identity, JobObjectAccessRights accessRights, AuditFlags type)
			: base(identity, (int) accessRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		public JobObjectAccessRights JobObjectRights
		{
			get
			{
				return (JobObjectAccessRights) AccessMask;
			}
		}
	}

	public sealed class JobObjectAccessRule : AccessRule
	{
		public JobObjectAccessRule(IdentityReference identity, JobObjectAccessRights accessRights, AccessControlType type)
			: base(identity, (int) accessRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		public JobObjectAccessRights JobObjectRights
		{
			get
			{
				return (JobObjectAccessRights) AccessMask;
			}
		}
	}

	public class JobObjectSecurity : CommonObjectSecurity
	{
		public JobObjectSecurity(bool isContainer)
			: base(isContainer)
		{
		}

		public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		{
			return new JobObjectAccessRule(identityReference, (JobObjectAccessRights) accessMask, type);
		}

		public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		{
			return new JobObjectAuditRule(identityReference, (JobObjectAccessRights) accessMask, flags);
		}

		public void AddAccessRule(IdentityReference identityReference, JobObjectAccessRights accessMask, AccessControlType type)
		{
			AddAccessRule(new JobObjectAccessRule(identityReference, accessMask, type));
		}

		public void AddAuditRule(IdentityReference identityReference, JobObjectAccessRights accessMask, AuditFlags flags)
		{
			AddAuditRule(new JobObjectAuditRule(identityReference, accessMask, flags));
		}

		public override Type AccessRightType
		{
			get
			{
				return typeof (JobObjectAccessRights);
			}
		}

		public override Type AccessRuleType
		{
			get
			{
				return typeof (JobObjectAccessRule);
			}
		}

		public override Type AuditRuleType
		{
			get
			{
				return typeof (JobObjectAuditRule);
			}
		}
	}
}
