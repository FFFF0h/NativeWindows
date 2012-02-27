namespace NativeWindows.JobObject
{
	public enum JobObjectType
	{
		BasicAccountingInformation = 1, // Query only
		BasicLimitInformation = 2,
		BasicProcessIdList = 3, // Query only
		BasicUIRestrictions = 4,
		SecurityLimitInformation = 5, // obsolete
		EndOfJobTimeInformation = 6,
		AssociateCompletionPortInformation = 7, // Set only
		BasicAndIoAccountingInformation = 8,
		ExtendedLimitInformation = 9,

		// Windows 7 / Windows Server 2008 R2 and forward
		GroupInformation = 11,

		// Next version (windows 8+)
		NotificationLimitInformation = 12,
		LimitViolationInformation = 13, // Query only
		GroupInformationEx = 14,
		CpuRateControlInformation = 15,
	}
}
