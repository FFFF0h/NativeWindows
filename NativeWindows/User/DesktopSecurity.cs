using System;
using System.Security.AccessControl;
using System.Security.Principal;

namespace NativeWindows.User
{
	public sealed class DesktopAuditRule : AuditRule
	{
		public DesktopAuditRule(IdentityReference identity, DesktopAccessRights accessRights, AuditFlags type)
			: base(identity, (int)accessRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		public DesktopAccessRights DesktopRights
		{
			get
			{
				return (DesktopAccessRights)AccessMask;
			}
		}
	}

	public sealed class DesktopAccessRule : AccessRule
	{
		public DesktopAccessRule(IdentityReference identity, DesktopAccessRights accessRights, AccessControlType type)
			: base(identity, (int)accessRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		public DesktopAccessRights DesktopRights
		{
			get
			{
				return (DesktopAccessRights)AccessMask;
			}
		}
	}

	public class DesktopSecurity : CommonObjectSecurity
	{
		public DesktopSecurity(bool isContainer)
			: base(isContainer)
		{
		}

		public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		{
			return new DesktopAccessRule(identityReference, (DesktopAccessRights)accessMask, type);
		}

		public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		{
			return new DesktopAuditRule(identityReference, (DesktopAccessRights)accessMask, flags);
		}

		public void AddAccessRule(IdentityReference identityReference, DesktopAccessRights accessMask, AccessControlType type)
		{
			AddAccessRule(new DesktopAccessRule(identityReference, accessMask, type));
		}

		public void AddAuditRule(IdentityReference identityReference, DesktopAccessRights accessMask, AuditFlags flags)
		{
			AddAuditRule(new DesktopAuditRule(identityReference, accessMask, flags));
		}

		public override Type AccessRightType
		{
			get
			{
				return typeof(DesktopAccessRights);
			}
		}

		public override Type AccessRuleType
		{
			get
			{
				return typeof(DesktopAccessRule);
			}
		}

		public override Type AuditRuleType
		{
			get
			{
				return typeof(DesktopAuditRule);
			}
		}
	}
}
