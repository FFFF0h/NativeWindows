using System;
using System.Security.Principal;

namespace NativeWindows.User
{
	public class SidAndAttributes
	{
		private readonly SidAttributes _attributes;
		private readonly SecurityIdentifier _securityIdentifier;

		public SidAndAttributes(SidAttributes attributes, IntPtr sid)
		{
			_attributes = attributes;
			_securityIdentifier = new SecurityIdentifier(sid);
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
