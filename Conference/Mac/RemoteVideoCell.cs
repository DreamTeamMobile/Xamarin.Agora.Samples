using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

namespace DT.Samples.Agora.Conference.iOS
{
    public partial class RemoteVideoCell : NSTableCellView
    {
        public NSView CellView => VideoContainerView;

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
                MuteAudioIcon.Hidden = !_isAudioMuted;
            }
        }

        private bool _isVideoMuted = false;
        public bool IsVideoMuted
        {
            get => _isVideoMuted;
            set
            {
                _isVideoMuted = value;
                MuteVideoIcon.Hidden = !_isVideoMuted;
                VideoContainerView.Hidden = _isVideoMuted;
            }
        }

        #region Constructors

        // Called when created from unmanaged code
        public RemoteVideoCell(IntPtr handle) : base(handle) { }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public RemoteVideoCell(NSCoder coder) : base(coder) { }

        public RemoteVideoCell() : base() { }

        #endregion
    }
}
