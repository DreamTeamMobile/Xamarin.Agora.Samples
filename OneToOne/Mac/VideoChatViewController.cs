using System;
using AppKit;
using Xamarin.Agora.Mac;
using CoreGraphics;
using DT.Samples.Agora.Shared;
using System.Threading.Tasks;

namespace DT.Samples.Agora.OneToOne.Mac
{
    /*
        For additional information on MacOS and Agora see Agora MacOS Swift tutorials
    */
    public partial class VideoChatViewController : NSViewController
    {
        protected const string Channel = "drmtm.us";
        private AgoraRtcEngineKit _agoraKit;
        private AgoraDelegate _agoraDelegate;
        private bool _muteAudio;
        private bool _muteVideo;
        private bool _screenShare;

        public VideoChatViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.View.WantsLayer = true;
            remoteVideo.WantsLayer = true;
            localVideo.WantsLayer = true;
            HideVideoMuted();
            InitializeAgoraEngine();
            SetupVideo();
            SetupLocalVideo();
            JoinChannel();
            LoadingIndicator.StartAnimation(this);
        }

        public override void ViewDidAppear()
        {
            base.ViewDidAppear();
            hungUpButton.Activated += didHungUpButtonClicked;
            muteButton.Activated += didClickedMuteButtonClicked;
            videoMuteButton.Activated += didVideoMuteButtonClicked;
            deviceSelectionButton.Activated += didDeviceSelectionButtonClicked;
            screenShareButton.Activated += didShareButtonClicked;
        }

        public override void ViewDidDisappear()
        {
            base.ViewDidDisappear();
            hungUpButton.Activated -= didHungUpButtonClicked;
            muteButton.Activated -= didClickedMuteButtonClicked;
            videoMuteButton.Activated -= didVideoMuteButtonClicked;
            deviceSelectionButton.Activated -= didDeviceSelectionButtonClicked;
            screenShareButton.Activated -= didShareButtonClicked;
        }

        private void didShareButtonClicked(object sender, EventArgs e)
        {
            _screenShare = !_screenShare;
            if (_screenShare)
            {
                (sender as NSButton).Image = NSImage.ImageNamed("screenShareButtonSelected");
                _agoraKit.StartScreenCapture(0, 15, 0, CGRect.Null);
            }
            else
            {
                (sender as NSButton).Image = NSImage.ImageNamed("screenShareButton");
                _agoraKit.StopScreenCapture();
            }
        }

        private void didDeviceSelectionButtonClicked(object sender, EventArgs e)
        {
            var deviceSelectionViewController = this.Storyboard.InstantiateControllerWithIdentifier("DeviceSelectionViewController") as DeviceSelectionViewController;
            this.PresentViewControllerAsSheet(deviceSelectionViewController);
            deviceSelectionViewController.SetAgoraKit(this._agoraKit);
        }

        private void didVideoMuteButtonClicked(object sender, EventArgs e)
        {
            _muteVideo = !_muteVideo;
            _agoraKit.MuteLocalVideoStream(_muteVideo);

            if (_muteVideo)
            {
                (sender as NSButton).Image = NSImage.ImageNamed("videoMuteButtonSelected");
            }
            else
            {
                (sender as NSButton).Image = NSImage.ImageNamed("videoMuteButton");
            }
        }

        private void didHungUpButtonClicked(object sender, EventArgs e)
        {
            LeaveChannel();
            Task.Delay(1000).ContinueWith(_ =>
            {
                BeginInvokeOnMainThread(() =>
                {
                    NSApplication.SharedApplication.Terminate(this);
                });
            });
        }

        private void didClickedMuteButtonClicked(object sender, EventArgs e)
        {
            _muteAudio = !_muteAudio;
            _agoraKit.MuteLocalAudioStream(_muteAudio);

            if (_muteAudio)
            {
                (sender as NSButton).Image = NSImage.ImageNamed("muteButtonSelected");
            }
            else
            {
                (sender as NSButton).Image = NSImage.ImageNamed("muteButton");
            }
        }

        public override void ViewWillAppear()
        {
            base.ViewWillAppear();
            View.Layer.BackgroundColor = NSColor.Black.CGColor;
            remoteVideo.Layer.BackgroundColor = NSColor.Clear.CGColor;
            localVideo.Layer.BackgroundColor = NSColor.Clear.CGColor;
        }

        public void InitializeAgoraEngine()
        {
            _agoraDelegate = new AgoraDelegate(this);
            _agoraKit = AgoraRtcEngineKit.SharedEngineWithAppId(AgoraTestConstants.AgoraAPI, _agoraDelegate);
        }

        public void SetupVideo()
        {
            _agoraKit.EnableVideo();
            _agoraKit.SetVideoProfile(Xamarin.Agora.Mac.VideoProfile.Landscape720P, swapWidthAndHeight: false);
        }

        public void SetupLocalVideo()
        {
            var videoCanvas = new AgoraRtcVideoCanvas();
            videoCanvas.Uid = 0;
            videoCanvas.View = localVideo;
            videoCanvas.RenderMode = VideoRenderMode.Adaptive;
            _agoraKit.SetupLocalVideo(videoCanvas);
            localVideoMutedIndicator.Hidden = true;
            localVideoMuteBg.Hidden = true;
        }

        public async Task JoinChannel()
        {
            LoadingIndicator.Hidden = false;
            var token = await AgoraTokenService.GetRtcToken(Channel);
            if (string.IsNullOrEmpty(token))
            {
                LoadingIndicator.Hidden = true;
            }
            else
            {
                _agoraKit.JoinChannelByToken(token, Channel, null, 0, (arg1, arg2, arg3) =>
                {
                    LoadingIndicator.Hidden = true;
                });
            }
        }

        public void LeaveChannel()
        {
            _agoraKit.LeaveChannel(null);
            _agoraKit.SetupLocalVideo(null);
            remoteVideo.RemoveFromSuperview();
            localVideo.RemoveFromSuperview();
            //delegate?.VideoChatNeedClose(self);
            _agoraKit = null;
        }

        public override void MouseMoved(NSEvent theEvent)
        {
            base.MouseMoved(theEvent);
            if (controlButtons.Hidden)
            {
                controlButtons.Hidden = false;
                //perform(#selector(hideControlButtons), with:nil, afterDelay:3)
            }
        }

        public override void MouseEntered(NSEvent theEvent)
        {
            base.MouseEntered(theEvent);
            //VideoChatViewController.cancelPreviousPerformRequests(withTarget: self)
        }

        public override void MouseExited(NSEvent theEvent)
        {
            base.MouseExited(theEvent);
            //perform(#selector(hideControlButtons), with:nil, afterDelay:3)
        }

        public void HideCOntrolButtons()
        {
            controlButtons.Hidden = true;
        }

        public void HideVideoMuted()
        {
            remoteVideoMutedIndicator.Hidden = true;
            localVideoMuteBg.Hidden = true;
            localVideoMutedIndicator.Hidden = true;
        }

        public void FirstRemoteVideoDecodedOfUid(AgoraRtcEngineKit engine, nuint uid, CGSize size, nint elapsed)
        {
            if (remoteVideo.Hidden)
            {
                remoteVideo.Hidden = false;
            }
            remoteVideoMutedIndicator.Hidden = true;
            var videoCanvas = new AgoraRtcVideoCanvas();
            videoCanvas.Uid = uid;
            videoCanvas.View = remoteVideo;
            videoCanvas.RenderMode = VideoRenderMode.Adaptive;
            _agoraKit.SetupRemoteVideo(videoCanvas);
        }

        public void DidOfflineOfUid(AgoraRtcEngineKit engine, nuint uid, UserOfflineReason reason)
        {
            remoteVideo.Hidden = true;
            remoteVideoMutedIndicator.Hidden = false;
        }

        public void DidVideoMuted(AgoraRtcEngineKit engine, bool muted, nuint uid)
        {
            remoteVideo.Hidden = muted;
            remoteVideoMutedIndicator.Hidden = !muted;
        }
    }
}
