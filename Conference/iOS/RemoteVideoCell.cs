using System;

using Foundation;
using UIKit;

namespace DT.Samples.Agora.Conference.iOS
{
    public partial class RemoteVideoCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("RemoteVideoCell");
        public static readonly UINib Nib;

        public UIView CellView => VideoContainer;

        private bool _isHandUp = false;
        public bool IsHandUp
        {
            get => _isHandUp;
            set
            {
                _isHandUp = value;
                HandUpImage.Hidden = !_isHandUp;
            }
        }

        private bool _isAudioMuted = false;
        public bool IsAudioMuted
        {
            get => _isAudioMuted;
            set
            {
                _isAudioMuted = value;
                MuteAudioView.Hidden = !_isAudioMuted;
            }
        }

        private bool _isVideoMuted = false;
        public bool IsVideoMuted
        {
            get => _isVideoMuted;
            set
            {
                _isVideoMuted = value;
                MuteVideoView.Hidden = !_isVideoMuted;
            }
        }

        protected RemoteVideoCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
