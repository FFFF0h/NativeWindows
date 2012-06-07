using System;

namespace NativeWindows.Identity
{
	public class LsaNamesResult : IDisposable
	{
		public LsaNamesResult(LsaReferencedDomainsHandle referencedDomainsHandle, LsaTranslatedSidHandle translatedSidHandle)
		{
			ReferencedDomainsHandle = referencedDomainsHandle;
			TranslatedSidHandle = translatedSidHandle;
		}

		public LsaReferencedDomainsHandle ReferencedDomainsHandle
		{
			get;
			private set;
		}

		public LsaTranslatedSidHandle TranslatedSidHandle
		{
			get;
			private set;
		}

		public void Dispose()
		{
			TranslatedSidHandle.Dispose();
			ReferencedDomainsHandle.Dispose();
		}
	}
}
