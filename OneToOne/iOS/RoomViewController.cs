using System;
using DT.Samples.Agora.Shared;
using DT.Samples.Agora.Shared.Helpers;
using DT.Xamarin.Agora;
using Foundation;
using UIKit;

namespace DT.Samples.Agora.OneToOne.iOS
{
    public partial class RoomViewController : UIViewController
    {
        public AgoraRtcDelegate AgoraDelegate;
        public AgoraRtcEngineKit AgoraKit;

        private bool _audioMuted = false;
        private bool _videoMuted = false;

        uint _localId = 0;
        uint _remoteId = 0;

        public bool AudioMuted
        {
            get
            {
                return _audioMuted;
            }
            set
            {
                _audioMuted = value;
                if (ToggleAudioButton != null)
                {
                    ToggleAudioButton.Selected = value;
                    AgoraKit.MuteLocalAudioStream(value);
                    UpdateMutedViewVisibility();
                }
            }
        }

        public bool VideoMuted
        {
            get
            {
                return _videoMuted;
            }
            set
            {
                _videoMuted = value;
                if (ToggleCamButton != null)
                {
                    ToggleCamButton.Selected = value;
                    LocalView.Hidden = value;
                    SwitchCamButton.Hidden = value;
                    AgoraKit.MuteLocalVideoStream(value);
                    UpdateMutedViewVisibility();
                }
            }
        }

        protected RoomViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationController.NavigationBarHidden = true;
            RoomNameLabel.Text = AgoraSettings.Current.RoomName;
            BackgroundTap.ShouldRequireFailureOfGestureRecognizer(BackgroundDoubleTap);
            AgoraDelegate = new AgoraRtcDelegate(this);
            AgoraKit = AgoraRtcEngineKit.SharedEngineWithAppIdAndDelegate(AgoraTestConstants.AgoraAPI, AgoraDelegate);
            AgoraKit.SetChannelProfile(ChannelProfile.Communication);
            AgoraKit.EnableVideo();
            //var profile = AgoraSettings.Current.UseMySettings ? AgoraSettings.Current.Profile.ToAgoraRtcVideoProfile() : DT.Xamarin.Agora.VideoProfile.Default;
            //AgoraKit.SetVideoProfile(profile, false);
            AgoraRtcVideoCanvas videoCanvas = new AgoraRtcVideoCanvas();
            videoCanvas.Uid = 0;
            videoCanvas.View = LocalView;
            LocalView.Hidden = false;
            videoCanvas.RenderMode = VideoRenderMode.Adaptive;
            AgoraKit.SetupLocalVideo(videoCanvas);
            if (!string.IsNullOrEmpty(AgoraSettings.Current.EncryptionPhrase))
            {
                AgoraKit.SetEncryptionMode(AgoraSettings.Current.EncryptionType.GetModeString());
                AgoraKit.SetEncryptionSecret(AgoraSettings.Current.EncryptionPhrase);
            }
            AgoraKit.StartPreview();
            AgoraKit.JoinChannelByToken(AgoraTestConstants.AgoraAPI, AgoraSettings.Current.RoomName, null, 0, JoiningCompleted);
        }

        private void JoiningCompleted(Foundation.NSString channel, nuint uid, nint elapsed)
        {
            _localId = (uint)uid;
            AgoraKit.SetEnableSpeakerphone(true);
            UIApplication.SharedApplication.IdleTimerDisabled = true;
            RefreshDebug();
        }

        public void FirstRemoteVideoDecodedOfUid(AgoraRtcEngineKit engine, nuint uid, CoreGraphics.CGSize size, nint elapsed)
        {
            _remoteId = (uint)uid;
            if (ContainerView.Hidden)
            {
                ContainerView.Hidden = false;
            }
            AgoraRtcVideoCanvas videoCanvas = new AgoraRtcVideoCanvas();
            videoCanvas.Uid = uid;
            videoCanvas.View = ContainerView;
            videoCanvas.RenderMode = VideoRenderMode.Adaptive;
            AgoraKit.SetupRemoteVideo(videoCanvas);
            if (ContainerView.Hidden)
            {
                ContainerView.Hidden = false;
            }
            RefreshDebug();
        }

        private void RefreshDebug()
        {
            DebugData.Text = $"local: {_localId}\nremote: {_remoteId}";
        }

        partial void ToggleAudioButtonClicked(NSObject sender)
        {
            AudioMuted = !AudioMuted;
        }

        partial void ToggleCamClicked(NSObject sender)
        {
            VideoMuted = !VideoMuted;
        }

        partial void SwitchCamClicked(NSObject sender)
        {
            AgoraKit.SwitchCamera();
        }

        public void LeaveChannel()
        {
            AgoraKit.LeaveChannel(null);
            NavigationController.NavigationBarHidden = false;
            NavigationController.PopViewController(true);
        }

        partial void EndCallClicked(NSObject sender)
        {
            LeaveChannel();
        }

        public void DidOfflineOfUid(AgoraRtcEngineKit engine, nuint uid, UserOfflineReason reason)
        {
            ContainerView.Hidden = true;
        }

        public void DidVideoMuted(AgoraRtcEngineKit engine, bool muted, nuint uid)
        {
            ContainerView.Hidden = muted;
        }

        public void FirstLocalVideoFrameWithSize(AgoraRtcEngineKit engine, CoreGraphics.CGSize size, nint elapsed)
        {
            var fixedSize = size.FixedSize(ContainerView.Bounds.Size);
            nfloat ratio;
            if (fixedSize.Width > 0 && fixedSize.Height > 0)
            {
                ratio = fixedSize.Width / fixedSize.Height;
            }
            else
            {
                ratio = ContainerView.Bounds.Width / ContainerView.Bounds.Height;
            }
            var viewWidth = NSLayoutConstraint.Create(LocalView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, ContainerView, NSLayoutAttribute.Width, 0.249999f, 0f);
            var viewRatio = NSLayoutConstraint.Create(LocalView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, LocalView, NSLayoutAttribute.Height, ratio, 0f);
            NSLayoutConstraint.DeactivateConstraints(new NSLayoutConstraint[] { LocalVideoWidth, LocalVideoHeight });
            NSLayoutConstraint.ActivateConstraints(new NSLayoutConstraint[] { viewWidth, viewRatio });
        }

        private void UpdateMutedViewVisibility()
        {
            if (!VideoMuted && AudioMuted)
            {
                MutedView.Hidden = false;
            }
            else
            {
                MutedView.Hidden = true;
            }
        }
    }
}
