using System;
using System.Runtime.InteropServices;

namespace NativeWindows.JobObject
{
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BasicUiRestrictions : IJobObjectSettable, IJobObjectQueryable
	{
		[Flags]
		private enum UiRestrictionsTypes : uint
		{
			LimitHandles = 0x00000001,
			LimitReadClipboard = 0x00000002,
			LimitWriteClipboard = 0x00000004,
			LimitSystemParameters = 0x00000008,
			LimitDisplaySettings = 0x00000010,
			LimitGlobalAtoms = 0x00000020,
			LimitDesktop = 0x00000040,
			LimitExitWindows = 0x00000080,
		}

		private UiRestrictionsTypes _restrictionsTypes;

		/// <summary>
		/// Prevents processes associated with the job from creating desktops and switching desktops using the CreateDesktop and SwitchDesktop functions.
		/// </summary>
		public bool LimitDesktop
		{
			get
			{
				return _restrictionsTypes.HasFlag(UiRestrictionsTypes.LimitDesktop);
			}
			set
			{
				ChangeFlag(value, UiRestrictionsTypes.LimitDesktop);
			}
		}

		/// <summary>
		/// Prevents processes associated with the job from calling the ChangeDisplaySettings function.
		/// </summary>
		public bool LimitDisplaySettings
		{
			get
			{
				return _restrictionsTypes.HasFlag(UiRestrictionsTypes.LimitDisplaySettings);
			}
			set
			{
				ChangeFlag(value, UiRestrictionsTypes.LimitDisplaySettings);
			}
		}

		/// <summary>
		/// Prevents processes associated with the job from calling the ExitWindows or ExitWindowsEx function.
		/// </summary>
		public bool LimitExitWindows
		{
			get
			{
				return _restrictionsTypes.HasFlag(UiRestrictionsTypes.LimitExitWindows);
			}
			set
			{
				ChangeFlag(value, UiRestrictionsTypes.LimitExitWindows);
			}
		}

		/// <summary>
		/// Prevents processes associated with the job from accessing global atoms. When this flag is used, each job has its own atom table.
		/// </summary>
		public bool LimitGlobalAtoms
		{
			get
			{
				return _restrictionsTypes.HasFlag(UiRestrictionsTypes.LimitGlobalAtoms);
			}
			set
			{
				ChangeFlag(value, UiRestrictionsTypes.LimitGlobalAtoms);
			}
		}

		/// <summary>
		/// Prevents processes associated with the job from using USER handles owned by processes not associated with the same job.
		/// </summary>
		public bool LimitHandles
		{
			get
			{
				return _restrictionsTypes.HasFlag(UiRestrictionsTypes.LimitHandles);
			}
			set
			{
				ChangeFlag(value, UiRestrictionsTypes.LimitHandles);
			}
		}

		/// <summary>
		/// Prevents processes associated with the job from reading data from the clipboard.
		/// </summary>
		public bool LimitReadClipboard
		{
			get
			{
				return _restrictionsTypes.HasFlag(UiRestrictionsTypes.LimitReadClipboard);
			}
			set
			{
				ChangeFlag(value, UiRestrictionsTypes.LimitReadClipboard);
			}
		}

		/// <summary>
		/// Prevents processes associated with the job from changing system parameters by using the SystemParametersInfo function.
		/// </summary>
		public bool LimitSystemParameters
		{
			get
			{
				return _restrictionsTypes.HasFlag(UiRestrictionsTypes.LimitSystemParameters);
			}
			set
			{
				ChangeFlag(value, UiRestrictionsTypes.LimitSystemParameters);
			}
		}

		/// <summary>
		/// Prevents processes associated with the job from writing data to the clipboard.
		/// </summary>
		public bool LimitWriteClipboard
		{
			get
			{
				return _restrictionsTypes.HasFlag(UiRestrictionsTypes.LimitWriteClipboard);
			}
			set
			{
				ChangeFlag(value, UiRestrictionsTypes.LimitWriteClipboard);
			}
		}

		public JobObjectType JobType
		{
			get
			{
				return JobObjectType.BasicUIRestrictions;
			}
		}

		private void ChangeFlag(bool set, UiRestrictionsTypes flag)
		{
			if (set)
			{
				_restrictionsTypes |= flag;
			}
			else
			{
				_restrictionsTypes &= ~flag;
			}
		}
	}
}
