using System;
using System.Security.AccessControl;
using System.Security.Principal;
using NativeWindows.Security;

namespace NativeWindows.IO
{
	public sealed class PipeAuditRule : AuditRule
	{
		public PipeAuditRule(IdentityReference identity, PipeAccessRights accessRights, AuditFlags type)
			: base(identity, (int)accessRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		public PipeAccessRights PipeRights
		{
			get
			{
				return (PipeAccessRights)AccessMask;
			}
		}
	}

	public sealed class PipeAccessRule : AccessRule
	{
		public PipeAccessRule(IdentityReference identity, PipeAccessRights accessRights, AccessControlType type)
			: base(identity, (int)accessRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		public PipeAccessRights PipeRights
		{
			get
			{
				return (PipeAccessRights)AccessMask;
			}
		}
	}

	public partial class PipeSecurity : SecurityBase
	{
		public PipeSecurity(bool isContainer = false)
			: base(isContainer, ResourceType.FileObject)
		{
		}

		public PipeSecurity(PipeHandle handle, AccessControlSections includeSections, bool isContainer = false)
			: base(isContainer, ResourceType.FileObject, handle, includeSections)
		{
		}

		public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		{
			return new PipeAccessRule(identityReference, (PipeAccessRights)accessMask, type);
		}

		public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		{
			return new PipeAuditRule(identityReference, (PipeAccessRights)accessMask, flags);
		}

		public void AddAccessRule(IdentityReference identityReference, PipeAccessRights accessMask, AccessControlType type)
		{
			AddAccessRule(new PipeAccessRule(identityReference, accessMask, type));
		}

		public void AddAuditRule(IdentityReference identityReference, PipeAccessRights accessMask, AuditFlags flags)
		{
			AddAuditRule(new PipeAuditRule(identityReference, accessMask, flags));
		}

		public override Type AccessRightType
		{
			get
			{
				return typeof(PipeAccessRights);
			}
		}

		public override Type AccessRuleType
		{
			get
			{
				return typeof(PipeAccessRule);
			}
		}

		public override Type AuditRuleType
		{
			get
			{
				return typeof(PipeAuditRule);
			}
		}
	}
}
