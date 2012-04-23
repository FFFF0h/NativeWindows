using System;
using System.Security.AccessControl;
using System.Security.Principal;
using NativeWindows.Security;

namespace NativeWindows.Processes
{
	public sealed class ProcessAuditRule : AuditRule
	{
		public ProcessAuditRule(IdentityReference identity, ProcessAccessRights accessRights, AuditFlags type)
			: base(identity, (int)accessRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		public ProcessAccessRights ProcessRights
		{
			get
			{
				return (ProcessAccessRights)AccessMask;
			}
		}
	}

	public sealed class ProcessAccessRule : AccessRule
	{
		public ProcessAccessRule(IdentityReference identity, ProcessAccessRights accessRights, AccessControlType type)
			: base(identity, (int)accessRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		public ProcessAccessRights ProcessRights
		{
			get
			{
				return (ProcessAccessRights)AccessMask;
			}
		}
	}

	public partial class ProcessSecurity : SecurityBase
	{
		public ProcessSecurity(bool isContainer = false)
			: base(isContainer, ResourceType.KernelObject)
		{
		}

		public ProcessSecurity(ProcessHandle handle, AccessControlSections includeSections, bool isContainer = false)
			: base(isContainer, ResourceType.KernelObject, handle, includeSections)
		{
		}

		public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		{
			return new ProcessAccessRule(identityReference, (ProcessAccessRights)accessMask, type);
		}

		public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		{
			return new ProcessAuditRule(identityReference, (ProcessAccessRights)accessMask, flags);
		}

		public void AddAccessRule(IdentityReference identityReference, ProcessAccessRights accessMask, AccessControlType type)
		{
			AddAccessRule(new ProcessAccessRule(identityReference, accessMask, type));
		}

		public void AddAuditRule(IdentityReference identityReference, ProcessAccessRights accessMask, AuditFlags flags)
		{
			AddAuditRule(new ProcessAuditRule(identityReference, accessMask, flags));
		}

		public override Type AccessRightType
		{
			get
			{
				return typeof(ProcessAccessRights);
			}
		}

		public override Type AccessRuleType
		{
			get
			{
				return typeof(ProcessAccessRule);
			}
		}

		public override Type AuditRuleType
		{
			get
			{
				return typeof(ProcessAuditRule);
			}
		}
	}
}
