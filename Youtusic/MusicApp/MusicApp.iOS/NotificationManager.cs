using System;
using MediaPlayer;
using MusicApp.Static;

namespace MusicApp.iOS
{
    public class NotificationManager : MediaManager.Platforms.Apple.Notifications.NotificationManager
	{
		public override bool ShowNavigationControls
		{
			get => base.ShowNavigationControls;
			set
			{
				base.ShowNavigationControls = value;
				// The base class attaches the commands.

				// Just override the settings here (the seeks, and skips, so the next en prev are shown)
				CommandCenter.SeekBackwardCommand.Enabled = false;
				CommandCenter.SeekForwardCommand.Enabled = false;
				CommandCenter.SkipBackwardCommand.Enabled = false;
				CommandCenter.SkipForwardCommand.Enabled = false;

				// Enable shuffle and repeat.
				CommandCenter.ChangeRepeatModeCommand.Enabled = true;
				CommandCenter.ChangeShuffleModeCommand.Enabled = true;
			}
		}

		protected override MPRemoteCommandHandlerStatus NextCommand(MPRemoteCommandEvent arg)
		{
			MediaController.Instance.RemovePosChangedHandler();
			return base.NextCommand(arg);
		}

		protected override MPRemoteCommandHandlerStatus PreviousCommand(MPRemoteCommandEvent arg)
		{
			MediaController.Instance.RemovePosChangedHandler();
			return base.PreviousCommand(arg);
		}
	}
}
