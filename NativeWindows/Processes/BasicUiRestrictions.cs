using System;
using System.Runtime.InteropServices;

namespace NativeWindows.Processes
{
	[StructLayout(LayoutKind.Sequential)]
	public sealed class BasicUiRestrictions : IJobObjectSettable, IJobObjectQueryable
	{
		[Flags]
		private enum UiRestrictionsTypes : uint
		{
			Handles = 0x00000001,
			ReadClipboard = 0x00000002,
			WriteClipboard = 0x00000004,
			SystemParameters = 0x00000008,
			DisplaySettings = 0x00000010,
			GlobalAtoms = 0x00000020,
			Desktop = 0x00000040,
			ExitWindows = 0x00000080,
		}

		private UiRestrictionsTypes _restrictionsTypes;

		/// <summary>
		/// Prevents processes associated with the job from creating desktops and switching desktops using the CreateDesktop and SwitchDesktop functions.
		/// </summary>
		public bool LimitDesktop
		{
			get
			{
				return _restrictionsTypes.HasFlag(UiRestrictionsTypes.Desktop);
			}
			set
			{
				ChangeFlag(value, UiRestrictionsTypes.Desktop);
			}
		}

		/// <summary>
		/// Prevents processes associated with the job from calling the ChangeDisplaySettings function.
		/// </summary>
		public bool LimitDisplaySettings
		{
			get
			{
				return _restrictionsTypes.HasFlag(UiRestrictionsTypes.DisplaySettings);
			}
			set
			{
				ChangeFlag(value, UiRestrictionsTypes.DisplaySettings);
			}
		}

		/// <summary>
		/// Prevents processes associated with the job from calling the ExitWindows or ExitWindowsEx function.
		/// </summary>
		public bool LimitExitWindows
		{
			get
			{
				return _restrictionsTypes.HasFlag(UiRestrictionsTypes.ExitWindows);
			}
			set
			{
				ChangeFlag(value, UiRestrictionsTypes.ExitWindows);
			}
		}

		/// <summary>
		/// Prevents processes associated with the job from accessing global atoms. When this flag is used, each job has its own atom table.
		/// </summary>
		public bool LimitGlobalAtoms
		{
			get
			{
				return _restrictionsTypes.HasFlag(UiRestrictionsTypes.GlobalAtoms);
			}
			set
			{
				ChangeFlag(value, UiRestrictionsTypes.GlobalAtoms);
			}
		}

		/// <summary>
		/// Prevents processes associated with the job from using USER handles owned by processes not associated with the same job.
		/// </summary>
		public bool LimitHandles
		{
			get
			{
				return _restrictionsTypes.HasFlag(UiRestrictionsTypes.Handles);
			}
			set
			{
				ChangeFlag(value, UiRestrictionsTypes.Handles);
			}
		}

		/// <summary>
		/// Prevents processes associated with the job from reading data from the clipboard.
		/// </summary>
		public bool LimitReadClipboard
		{
			get
			{
				return _restrictionsTypes.HasFlag(UiRestrictionsTypes.ReadClipboard);
			}
			set
			{
				ChangeFlag(value, UiRestrictionsTypes.ReadClipboard);
			}
		}

		/// <summary>
		/// Prevents processes associated with the job from changing system parameters by using the SystemParametersInfo function.
		/// </summary>
		public bool LimitSystemParameters
		{
			get
			{
				return _restrictionsTypes.HasFlag(UiRestrictionsTypes.SystemParameters);
			}
			set
			{
				ChangeFlag(value, UiRestrictionsTypes.SystemParameters);
			}
		}

		/// <summary>
		/// Prevents processes associated with the job from writing data to the clipboard.
		/// </summary>
		public bool LimitWriteClipboard
		{
			get
			{
				return _restrictionsTypes.HasFlag(UiRestrictionsTypes.WriteClipboard);
			}
			set
			{
				ChangeFlag(value, UiRestrictionsTypes.WriteClipboard);
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
