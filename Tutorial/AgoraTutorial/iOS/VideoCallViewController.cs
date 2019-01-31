using System;
using DT.Xamarin.Agora;
using Foundation;
using UIKit;

namespace AgoraTutorial.iOS
{
    public partial class VideoCallViewController : UIViewController, IAgoraRtcEngineDelegate
    {
        AgoraRtcEngineKit agoraKit;
        string AppID = "Your-App-ID";
        public static string Channel = ""; //User inputted channel name from VC (steps come later in tutorial)

        public VideoCallViewController(IntPtr handle) : base(handle)
        {
        }

        public void FirstRemoteVideoDecodedOfUid()
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            InitializeAgoraEngine();
            SetupVideo();
            JoinChannel();
            SetupLocalVideo();
            HideVideoMuted();
            SetupButtons();
        }

        void InitializeAgoraEngine()
        {
            agoraKit = AgoraRtcEngineKit.SharedEngineWithAppIdAndDelegate(AppID, this);
        }

        void SetupVideo()
        {
            agoraKit.EnableVideo();  // Enables video mode.
            agoraKit.SetVideoProfile(VideoProfile.Portrait360P, false); // Default video profile is 360P
        }

        void JoinChannel()
        {
            agoraKit.JoinChannelByToken(null, Channel, null, 0, (sid, uid, elapsed) =>
            {
                agoraKit.SetEnableSpeakerphone(true);
                UIApplication.SharedApplication.IdleTimerDisabled = true;
            });
        }

        void SetupLocalVideo()
        {
            var videoCanvas = new AgoraRtcVideoCanvas();
            videoCanvas.Uid = 0;
            videoCanvas.View = localVideo;
            videoCanvas.RenderMode = VideoRenderMode.Fit;
            agoraKit.SetupLocalVideo(videoCanvas);
        }

        [Export("rtcEngine:firstRemoteVideoDecodedOfUid:size:elapsed:")]
        public void FirstRemoteVideoDecodedOfUid(AgoraRtcEngineKit engine, nuint uid, CoreGraphics.CGSize size, nint elapsed)
        {
            if (remoteVideo.Hidden)
            {
                remoteVideo.Hidden = false;
            }
            var videoCanvas = new AgoraRtcVideoCanvas();
            videoCanvas.Uid = uid;
            videoCanvas.View = remoteVideo;
            videoCanvas.RenderMode = VideoRenderMode.Adaptive;
            agoraKit.SetupRemoteVideo(videoCanvas);
        }

        [Export("rtcEngine:didOfflineOfUid:reason:")]
        public void DidOfflineOfUid(AgoraRtcEngineKit engine, nuint uid, UserOfflineReason reason)
        {
            remoteVideo.Hidden = true;
        }

        [Export("rtcEngine:didVideoMuted:byUid:")]
        public void DidVideoMuted(AgoraRtcEngineKit engine, bool muted, nuint uid)
        {
            remoteVideo.Hidden = muted;
            remoteVideoMutedIndicator.Hidden = !muted;
        }

        partial void HangUpButtonClick(NSObject sender)
        {
            LeaveChannel();
        }

        void LeaveChannel()
        {
            agoraKit.LeaveChannel(null);
            HideControlButtons();
            UIApplication.SharedApplication.IdleTimerDisabled = false;
            remoteVideo.RemoveFromSuperview();
            localVideo.RemoveFromSuperview();
            agoraKit = null;
            this.DismissViewController(true, null);
        }

        void HideControlButtons()
        {
            controlButtons.Hidden = true;
        }

        partial void MuteButtonClick(NSObject sender)
        {
            var button = (UIButton)sender;
            button.Selected = !button.Selected;
            agoraKit.MuteLocalAudioStream(button.Selected);
        }

        partial void SwitchCameraButtonClick(NSObject sender)
        {
            var button = (UIButton)sender;
            button.Selected = !button.Selected;
            agoraKit.SwitchCamera();
        }

        partial void VideoMuteButtonClick(NSObject sender)
        {
            var button = (UIButton)sender;
            button.Selected = !button.Selected;
            agoraKit.MuteLocalVideoStream(button.Selected);
            localVideo.Hidden = button.Selected;
            localVideoMutedBg.Hidden = !button.Selected;
            localVideoMutedIndicator.Hidden = !button.Selected;
        }

        void HideVideoMuted()
        {
            remoteVideoMutedIndicator.Hidden = true;
            localVideoMutedBg.Hidden = true;
            localVideoMutedIndicator.Hidden = true;
        }

        void SetupButtons()
        {
            var tapGestureRecognizer = new UITapGestureRecognizer(ViewTapped);
            View.AddGestureRecognizer(tapGestureRecognizer);
            View.UserInteractionEnabled = true;
        }

        void ViewTapped()
        {
            controlButtons.Hidden = !controlButtons.Hidden;
        }
    }
}
