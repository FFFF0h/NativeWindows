using System;
using System.Security.AccessControl;
using System.Security.Principal;

namespace NativeWindows.IO
{
	public sealed class FileAuditRule : AuditRule
	{
		public FileAuditRule(IdentityReference identity, FileAccessRights accessRights, AuditFlags type)
			: base(identity, (int)accessRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		public FileAccessRights FileRights
		{
			get
			{
				return (FileAccessRights)AccessMask;
			}
		}
	}

	public sealed class FileAccessRule : AccessRule
	{
		public FileAccessRule(IdentityReference identity, FileAccessRights accessRights, AccessControlType type)
			: base(identity, (int)accessRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		public FileAccessRights FileRights
		{
			get
			{
				return (FileAccessRights)AccessMask;
			}
		}
	}

	public class FileSecurity : SecurityBase
	{
		public FileSecurity(bool isContainer = false)
			: base(isContainer, ResourceType.FileObject)
		{
		}

		public FileSecurity(FileHandle handle, AccessControlSections includeSections, bool isContainer = false)
			: base(isContainer, ResourceType.FileObject, handle, includeSections)
		{
		}

		public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		{
			return new FileAccessRule(identityReference, (FileAccessRights)accessMask, type);
		}

		public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		{
			return new FileAuditRule(identityReference, (FileAccessRights)accessMask, flags);
		}

		public void AddAccessRule(IdentityReference identityReference, FileAccessRights accessMask, AccessControlType type)
		{
			AddAccessRule(new FileAccessRule(identityReference, accessMask, type));
		}

		public void AddAuditRule(IdentityReference identityReference, FileAccessRights accessMask, AuditFlags flags)
		{
			AddAuditRule(new FileAuditRule(identityReference, accessMask, flags));
		}

		public override Type AccessRightType
		{
			get
			{
				return typeof(FileAccessRights);
			}
		}

		public override Type AccessRuleType
		{
			get
			{
				return typeof(FileAccessRule);
			}
		}

		public override Type AuditRuleType
		{
			get
			{
				return typeof(FileAuditRule);
			}
		}
	}
}
