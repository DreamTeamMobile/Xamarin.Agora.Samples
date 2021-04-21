using System;
using AppKit;
using Xamarin.Agora.Mac;
using CoreGraphics;
using DT.Samples.Agora.Shared;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using DT.Xamarin.Agora;
using Newtonsoft.Json;
using VideoProfile = Xamarin.Agora.Mac.VideoProfile;

namespace DT.Samples.Agora.Conference.iOS
{
    /*
        For additional information on MacOS and Agora see Agora MacOS Swift tutorials
    */
    public partial class VideoChatViewController : NSViewController
    {
        protected const string Channel = "drmtm.us";
        private AgoraRtcEngineKit _agoraKit;
        private RtcDelegate _agoraDelegate;
        private bool _muteAudio;
        private bool _muteVideo;
        private bool _screenShare;
        private bool _handUp = false;
        private uint _localId;
        private List<RemoteVideoInfo> _users = new List<RemoteVideoInfo>();
        private AgoraRtmKit _rtmKit = new AgoraRtmKit(AgoraTestConstants.AgoraAPI, null);
        private AgoraRtmChannel _rtmChannel;

        public VideoChatViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.View.WantsLayer = true;
            localVideo.WantsLayer = true;
            HideVideoMuted();
            InitializeAgoraEngine();
            var dataSource = new RemoteVideosTableDataSource(_users);
            RemoteUsersTableView.DataSource = dataSource;
            RemoteUsersTableView.Delegate = new RemoteVideosTableDelegate(dataSource, _agoraKit);
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
            handUpButton.Activated += didHandUpButtonCLicked;
        }

        public override void ViewDidDisappear()
        {
            base.ViewDidDisappear();
            hungUpButton.Activated -= didHungUpButtonClicked;
            muteButton.Activated -= didClickedMuteButtonClicked;
            videoMuteButton.Activated -= didVideoMuteButtonClicked;
            deviceSelectionButton.Activated -= didDeviceSelectionButtonClicked;
            screenShareButton.Activated -= didShareButtonClicked;
            handUpButton.Activated -= didHandUpButtonCLicked;
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
        
        private void didHandUpButtonCLicked(object sender, EventArgs e)
        {
            _handUp = !_handUp;

            if (_handUp)
            {
                (sender as NSButton).Image = NSImage.ImageNamed("handUpSelected");
            }
            else
            {
                (sender as NSButton).Image = NSImage.ImageNamed("handUp");
            }

            var signalMessage = new SignalMessage
            {
                Action = _handUp ? SignalActionTypes.HandUp : SignalActionTypes.HandDown,
                RtcPeerId = _localId
            };
            var text = JsonConvert.SerializeObject(signalMessage);
            var rtmMessage = new AgoraRtmMessage(text);
            _rtmChannel.SendMessage(rtmMessage, (state) =>
            {
                Console.WriteLine($"RTM send channel msg state: {state}");
            });
        }

        public override void ViewWillAppear()
        {
            base.ViewWillAppear();
            View.Layer.BackgroundColor = NSColor.Black.CGColor;
            localVideo.Layer.BackgroundColor = NSColor.Clear.CGColor;
        }

        public void InitializeAgoraEngine()
        {
            _agoraDelegate = new RtcDelegate(this);
            _agoraKit = AgoraRtcEngineKit.SharedEngineWithAppId(AgoraTestConstants.AgoraAPI, _agoraDelegate);
        }

        public void SetupVideo()
        {
            _agoraKit.EnableVideo();
            _agoraKit.SetVideoProfile(VideoProfile.Landscape720P, swapWidthAndHeight: false);
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
                    _localId = (uint)arg2;
                    JoinRtm(arg2.ToString());
                });
            }
        }

        private async Task JoinRtm(string account)
        {
            var token = await AgoraTokenService.GetRtmToken(account);
            _rtmKit.LoginByToken(token, account, (status) =>
            {
                if (status == AgoraRtmLoginErrorCode.Ok)
                {
                    InvokeOnMainThread(() =>
                    {
                        var rtmDelegate = new RtmDelegate();
                        rtmDelegate.OnMessageReceived += OmMessageReceived;
                        _rtmKit.AgoraRtmDelegate = rtmDelegate;

                        var channelDelegate = new RtmChannelDelegate();
                        channelDelegate.OnMessageReceived += OmMessageReceived;
                        channelDelegate.ShowAlert += (user, msg) => Console.WriteLine($"RTM alert. {user}: {msg}"); ;

                        _rtmChannel = _rtmKit.CreateChannelWithId(Channel, channelDelegate);

                        if (_rtmChannel == null)
                            return;

                        _rtmChannel.JoinWithCompletion(JoinChannelBlock);
                    });
                }
            });
        }

        private void JoinChannelBlock(AgoraRtmJoinChannelErrorCode errorCode)
        {
            if (errorCode == AgoraRtmJoinChannelErrorCode.Ok)
            {
                Console.WriteLine($"RTM join channel successsful");
                handUpButton.Enabled = true;
            }
            else
            {
                Console.WriteLine($"RTM join channel error: {errorCode}");
            }
        }

        private void OmMessageReceived(string peerId, AgoraRtmMessage message)
        {
            var text = message.Text;
            var signal = JsonConvert.DeserializeObject<SignalMessage>(text);
            var userItemIndex = _users.IndexOf(_users.First(i => i.Uid == signal.RtcPeerId));
            switch (signal.Action)
            {
                case SignalActionTypes.HandDown:
                    _users[userItemIndex].IsHandUp = false;
                    break;
                case SignalActionTypes.HandUp:
                    _users[userItemIndex].IsHandUp = true;
                    break;
            }
            RemoteUsersTableView.ReloadData();
        }

        public void LeaveChannel()
        {
            _agoraKit.LeaveChannel(null);
            _agoraKit.SetupLocalVideo(null);
            RemoteUsersTableView.RemoveFromSuperview();
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
            localVideoMuteBg.Hidden = true;
            localVideoMutedIndicator.Hidden = true;
        }

        public void FirstRemoteVideoDecodedOfUid(AgoraRtcEngineKit engine, nuint uid, CGSize size, nint elapsed)
        {
            if (_users.Any(u => u.Uid == uid))
                return;
            _users.Add(new RemoteVideoInfo { Uid = (uint)uid });
            RemoteUsersTableView.ReloadData();
        }

        public void DidOfflineOfUid(AgoraRtcEngineKit engine, nuint uid, UserOfflineReason reason)
        {
            _users.Remove(_users.First(u => u.Uid == (uint)uid));
            RemoteUsersTableView.ReloadData();
        }

        public void DidVideoMuted(AgoraRtcEngineKit engine, bool muted, nuint uid)
        {
            var index = _users.IndexOf(_users.First(u => u.Uid == (uint)uid));
            _users[index].IsVideoMuted = muted;
            RemoteUsersTableView.ReloadData();
        }

        public void DidAudioMuted(AgoraRtcEngineKit engine, bool muted, nuint uid)
        {
            var index = _users.IndexOf(_users.First(u => u.Uid == (uint)uid));
            _users[index].IsAudioMuted = muted;
            RemoteUsersTableView.ReloadData();
        }

        public async Task TokenPrivilegeWillExpire(AgoraRtcEngineKit engine, string token)
        {
            var newToken = await AgoraTokenService.GetRtcToken(Channel);
            if (!string.IsNullOrEmpty(newToken))
            {
                _agoraKit.RenewToken(newToken);
            }
        }
    }
}
