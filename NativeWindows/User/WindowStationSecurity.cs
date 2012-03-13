using System;
using System.Security.AccessControl;
using System.Security.Principal;

namespace NativeWindows.User
{
	public sealed class WindowStationAuditRule : AuditRule
	{
		public WindowStationAuditRule(IdentityReference identity, WindowStationAccessRights accessRights, AuditFlags type)
			: base(identity, (int)accessRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		public WindowStationAccessRights WindowStationRights
		{
			get
			{
				return (WindowStationAccessRights)AccessMask;
			}
		}
	}

	public sealed class WindowStationAccessRule : AccessRule
	{
		public WindowStationAccessRule(IdentityReference identity, WindowStationAccessRights accessRights, AccessControlType type)
			: base(identity, (int)accessRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		public WindowStationAccessRights WindowStationRights
		{
			get
			{
				return (WindowStationAccessRights)AccessMask;
			}
		}
	}

	public class WindowStationSecurity : SecurityBase
	{
		public WindowStationSecurity(bool isContainer = false)
			: base(isContainer, ResourceType.WindowObject)
		{
		}

		public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		{
			return new WindowStationAccessRule(identityReference, (WindowStationAccessRights)accessMask, type);
		}

		public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		{
			return new WindowStationAuditRule(identityReference, (WindowStationAccessRights)accessMask, flags);
		}

		public void AddAccessRule(IdentityReference identityReference, WindowStationAccessRights accessMask, AccessControlType type)
		{
			AddAccessRule(new WindowStationAccessRule(identityReference, accessMask, type));
		}

		public void AddAuditRule(IdentityReference identityReference, WindowStationAccessRights accessMask, AuditFlags flags)
		{
			AddAuditRule(new WindowStationAuditRule(identityReference, accessMask, flags));
		}

		public override Type AccessRightType
		{
			get
			{
				return typeof(WindowStationAccessRights);
			}
		}

		public override Type AccessRuleType
		{
			get
			{
				return typeof(WindowStationAccessRule);
			}
		}

		public override Type AuditRuleType
		{
			get
			{
				return typeof(WindowStationAuditRule);
			}
		}
	}
}
