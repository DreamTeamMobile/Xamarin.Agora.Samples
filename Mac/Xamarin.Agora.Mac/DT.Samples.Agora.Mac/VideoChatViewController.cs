using System;
using AppKit;
using Xamarin.Agora.Mac;
using CoreGraphics;

namespace XMBindingExample
{
	public partial class VideoChatViewController : NSViewController
	{
        protected const string Channel = "drmtm.us";
        protected const string AgoraKey = "99e01787b7be41fb9dc269e525f2395f";
        private AgoraRtcEngineKit _agoraKit;
        private AgoraDelegate _agoraDelegate;
        private bool _muteAudio;
        private bool _muteVideo;
        private bool _screenShare;

        public VideoChatViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.View.WantsLayer = true;
            remoteVideo.WantsLayer = true;
            localVideo.WantsLayer = true;

            SetupButtons();              
            HideVideoMuted();            
            InitializeAgoraEngine();     
            SetupVideo();                
            SetupLocalVideo();           
            JoinChannel();
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
            if(_screenShare)
            {
                //(sender as NSButton).Image = new NSImage("screenShareButtonSelected"); 
                _agoraKit.StartScreenCapture(0, 15, 0, CGRect.Empty);
            }
            else
            {
                //(sender as NSButton).Image = new NSImage("screenShareButton"); 
                _agoraKit.StopScreenCapture();
            }
        }

        private void didDeviceSelectionButtonClicked(object sender, EventArgs e)
        {
            var deviceSelectionViewController = this.Storyboard.InstantiateControllerWithIdentifier("DeviceSelectionViewController") as NSViewController;
            this.PresentViewControllerAsSheet(deviceSelectionViewController);
        }

        private void didVideoMuteButtonClicked(object sender, EventArgs e)
        {
            _muteVideo = !_muteVideo;
            _agoraKit.MuteLocalVideoStream(_muteVideo);

            if (_muteAudio)
            {
                //(sender as NSButton).Image = new NSImage("videoMuteButtonSelected"); 
            }
            else
            {
                //(sender as NSButton).Image = new NSImage("videoMuteButton");
            }
        }

        private void didHungUpButtonClicked(object sender, EventArgs e)
        {
            LeaveChannel();
        }

        private void didClickedMuteButtonClicked(object sender, EventArgs e)
        {
            _muteAudio = !_muteAudio;
            _agoraKit.MuteLocalAudioStream(_muteAudio);

            if(_muteAudio)
            {
                //(sender as NSButton).Image = new NSImage("muteButtonSelected"); 
            }
            else
            {
                //(sender as NSButton).Image = new NSImage("muteButton");
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
            _agoraKit = AgoraRtcEngineKit.SharedEngineWithAppId(AgoraKey, _agoraDelegate);
        }

        public void SetupVideo()
        {
            _agoraKit.EnableVideo();
            _agoraKit.SetVideoProfile(AgoraVideoProfile.Landscape720P, swapWidthAndHeight: false);
        }

        public void SetupLocalVideo()
        {
            var videoCanvas = new AgoraRtcVideoCanvas();
            videoCanvas.Uid = 0;
            videoCanvas.View = localVideo;
            videoCanvas.RenderMode = AgoraVideoRenderMode.Adaptive;
            _agoraKit.SetupLocalVideo(videoCanvas);
            localVideoMutedIndicator.Hidden = true;
            localVideoMuteBg.Hidden = true;
        }

        public void JoinChannel()
        {
            _agoraKit.JoinChannelByToken(null, Channel, null, 0, (arg1, arg2, arg3) => {});
        }

        public void LeaveChannel()
        {
            _agoraKit.LeaveChannel(null);
            _agoraKit.SetupLocalVideo(null);
            remoteVideo.RemoveFromSuperview();
            localVideo.RemoveFromSuperview();
            //delegate?.VideoChatNeedClose(self);
            _agoraKit = null;
            View.Window?.Close();
        }


        public void SetupButtons()
        {
            //perform(#selector(hideControlButtons), with:nil, afterDelay:3)
        //    var remoteVideoTrackingArea = new NSTrackingArea(remoteVideo.Bounds, 
        //                                                       NSTrackingAreaOptions.MouseMoved & NSTrackingAreaOptions.ActiveInActiveApp & NSTrackingAreaOptions.InVisibleRect, 
        //                                                       this, 
        //                                                       null);

        //    remoteVideo.AddTrackingArea(remoteVideoTrackingArea);

        //    var controlButtonsTrackingArea = new NSTrackingArea(controlButtons.Bounds,
        //                                                        NSTrackingAreaOptions.MouseEnteredAndExited & NSTrackingAreaOptions.ActiveInActiveApp & NSTrackingAreaOptions.InVisibleRect,
        //                                                       this,
        //                                                       null);

        //    controlButtons.AddTrackingArea(controlButtonsTrackingArea);
        }

        public override void MouseMoved(NSEvent theEvent)
        {
            base.MouseMoved(theEvent);
            if(controlButtons.Hidden)
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
            videoCanvas.RenderMode = AgoraVideoRenderMode.Adaptive;
            _agoraKit.SetupRemoteVideo(videoCanvas);
        }

        public void DidOfflineOfUid(AgoraRtcEngineKit engine, nuint uid, AgoraUserOfflineReason reason)
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
