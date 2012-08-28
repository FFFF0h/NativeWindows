using System;
using System.Security.Principal;
using NativeWindows.Identity;

namespace NativeWindows.Security
{
	public class SidAndAttributes
	{
		private readonly SidAttributes _attributes;
		private readonly SecurityIdentifier _securityIdentifier;

		public SidAndAttributes(SidAttributes attributes, IntPtr sid)
		{
			_securityIdentifier = new SecurityIdentifier(sid);
			_attributes = attributes;
		}

		public SidAttributes Attributes
		{
			get
			{
				return _attributes;
			}
		}

		public SecurityIdentifier SecurityIdentifier
		{
			get
			{
				return _securityIdentifier;
			}
		}
	}
}
