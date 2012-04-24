using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace NativeWindows
{
	[Serializable]
	public partial class AccessDeniedException : Win32Exception
	{
		public AccessDeniedException()
			: base((int)SystemErrorCode.ErrorAccessDenied)
		{
		}

		protected AccessDeniedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

}
