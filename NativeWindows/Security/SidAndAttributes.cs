using System;
using System.Security.Principal;
using NativeWindows.Identity;

namespace NativeWindows.Security
{
	public class SidAndAttributes
	{
		private readonly SidHandle _sid;
		private readonly SidAttributes _attributes;

		public SidAndAttributes(SidAttributes attributes, SidHandle sid)
		{
			_attributes = attributes;
			_sid = sid;
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
				return new SecurityIdentifier(_sid.DangerousGetHandle());
			}
		}

		public SidHandle SidHandle
		{
			get
			{
				return _sid;
			}
		}
	}
}
