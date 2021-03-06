﻿using System;
using System.Security.AccessControl;
using System.Security.Principal;
using NativeWindows.Security;

namespace NativeWindows.IO
{
	public sealed class DirectoryAuditRule : AuditRule
	{
		public DirectoryAuditRule(IdentityReference identity, DirectoryAccessRights accessRights, AuditFlags type)
			: base(identity, (int)accessRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		public DirectoryAccessRights DirectoryRights
		{
			get
			{
				return (DirectoryAccessRights)AccessMask;
			}
		}
	}

	public sealed class DirectoryAccessRule : AccessRule
	{
		public DirectoryAccessRule(IdentityReference identity, DirectoryAccessRights accessRights, AccessControlType type)
			: base(identity, (int)accessRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		public DirectoryAccessRights DirectoryRights
		{
			get
			{
				return (DirectoryAccessRights)AccessMask;
			}
		}
	}

	public partial class DirectorySecurity : SecurityBase
	{
		public DirectorySecurity(bool isContainer = false)
			: base(isContainer, ResourceType.FileObject)
		{
		}

		public DirectorySecurity(DirectoryHandle handle, AccessControlSections includeSections, bool isContainer = false)
			: base(isContainer, ResourceType.FileObject, handle, includeSections)
		{
		}

		public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		{
			return new DirectoryAccessRule(identityReference, (DirectoryAccessRights)accessMask, type);
		}

		public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		{
			return new DirectoryAuditRule(identityReference, (DirectoryAccessRights)accessMask, flags);
		}

		public void AddAccessRule(IdentityReference identityReference, DirectoryAccessRights accessMask, AccessControlType type)
		{
			AddAccessRule(new DirectoryAccessRule(identityReference, accessMask, type));
		}

		public void AddAuditRule(IdentityReference identityReference, DirectoryAccessRights accessMask, AuditFlags flags)
		{
			AddAuditRule(new DirectoryAuditRule(identityReference, accessMask, flags));
		}

		public override Type AccessRightType
		{
			get
			{
				return typeof(DirectoryAccessRights);
			}
		}

		public override Type AccessRuleType
		{
			get
			{
				return typeof(DirectoryAccessRule);
			}
		}

		public override Type AuditRuleType
		{
			get
			{
				return typeof(DirectoryAuditRule);
			}
		}
	}
}
