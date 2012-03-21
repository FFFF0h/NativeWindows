using System;
using System.Security.AccessControl;
using System.Security.Principal;

namespace NativeWindows.ProcessAndThread
{
	public sealed class ThreadAuditRule : AuditRule
	{
		public ThreadAuditRule(IdentityReference identity, ThreadAccessRights accessRights, AuditFlags type)
			: base(identity, (int)accessRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		public ThreadAccessRights ThreadRights
		{
			get
			{
				return (ThreadAccessRights)AccessMask;
			}
		}
	}

	public sealed class ThreadAccessRule : AccessRule
	{
		public ThreadAccessRule(IdentityReference identity, ThreadAccessRights accessRights, AccessControlType type)
			: base(identity, (int)accessRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		public ThreadAccessRights ThreadRights
		{
			get
			{
				return (ThreadAccessRights)AccessMask;
			}
		}
	}

	public class ThreadSecurity : SecurityBase
	{
		public ThreadSecurity(bool isContainer = false)
			: base(isContainer, ResourceType.KernelObject)
		{
		}

		public ThreadSecurity(ThreadHandle handle, AccessControlSections includeSections, bool isContainer = false)
			: base(isContainer, ResourceType.KernelObject, handle, includeSections)
		{
		}

		public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		{
			return new ThreadAccessRule(identityReference, (ThreadAccessRights)accessMask, type);
		}

		public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		{
			return new ThreadAuditRule(identityReference, (ThreadAccessRights)accessMask, flags);
		}

		public void AddAccessRule(IdentityReference identityReference, ThreadAccessRights accessMask, AccessControlType type)
		{
			AddAccessRule(new ThreadAccessRule(identityReference, accessMask, type));
		}

		public void AddAuditRule(IdentityReference identityReference, ThreadAccessRights accessMask, AuditFlags flags)
		{
			AddAuditRule(new ThreadAuditRule(identityReference, accessMask, flags));
		}

		public override Type AccessRightType
		{
			get
			{
				return typeof(ThreadAccessRights);
			}
		}

		public override Type AccessRuleType
		{
			get
			{
				return typeof(ThreadAccessRule);
			}
		}

		public override Type AuditRuleType
		{
			get
			{
				return typeof(ThreadAuditRule);
			}
		}
	}
}
