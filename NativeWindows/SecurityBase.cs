using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace NativeWindows
{
	public abstract class SecurityBase : NativeObjectSecurity
	{
		protected SecurityBase(bool isContainer, ResourceType resourceType)
			: base(isContainer, resourceType)
		{
		}

		protected SecurityBase(bool isContainer, ResourceType resourceType, SafeHandle handle, AccessControlSections includeSections)
			: base(isContainer, resourceType, handle, includeSections)
		{
		}

		protected AccessControlSections GetIncludedSections()
		{
			var sectionsChanged = AccessControlSections.None;
			if (OwnerModified)
			{
				sectionsChanged |= AccessControlSections.Owner;
			}
			if (GroupModified)
			{
				sectionsChanged |= AccessControlSections.Group;
			}
			if (AccessRulesModified)
			{
				sectionsChanged |= AccessControlSections.Access;
			}
			if (AuditRulesModified)
			{
				sectionsChanged |= AccessControlSections.Audit;
			}
			return sectionsChanged;
		}

		public void ApplyChangesTo(SafeHandle handle)
		{
			WriteLock();
			try
			{
				Persist(handle, GetIncludedSections());
			}
			finally
			{
				WriteUnlock();
			}
		}
	}
}
