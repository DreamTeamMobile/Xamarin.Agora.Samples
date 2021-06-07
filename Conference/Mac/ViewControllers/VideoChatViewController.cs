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
        protected const string Username = "MacUser";
        private AgoraRtcEngineKit _agoraKit;
        private RtcDelegate _agoraDelegate;
        private bool _muteAudio;
        private bool _muteVideo;
        private bool _screenShare;
        private bool _handUp = false;
        private uint _localId;
        private List<RemoteVideoInfo> _users = new List<RemoteVideoInfo>();

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
            SetupVideo();
            ResetButtons();
            SetupLocalVideo();
            JoinChannel();
            RtmService.Instance.OnLogin += OnRtmLogin;
            RtmService.Instance.OnJoinChannel += OnRtmJoin;
            RtmService.Instance.OnSignalReceived += OmMessageReceived;
            InitAndJoinRtm();
            LoadingIndicator.StartAnimation(this);
        }

        private void OnRtmJoin(bool success)
        {
            handUpButton.Enabled = success;
        }

        private void OnRtmLogin(bool success)
        {
            if(success)
            {
                userNameLabel.StringValue = $"User name: {Username}";
                RtmService.Instance.JoinChannel(Channel);
            }
        }

        private async Task InitAndJoinRtm()
        {
            await RtmService.Instance.Init(Username);
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
            inviteButton.Activated += didInviteButtonClicked;
        }

        private void ResetButtons()
        {
            muteButton.Image = NSImage.ImageNamed("muteButton");
            videoMuteButton.Image = NSImage.ImageNamed("videoMuteButton");
            handUpButton.Image = NSImage.ImageNamed("handUp");
        }

        private void didInviteButtonClicked(object sender, EventArgs e)
        {
            var alert = new NSAlert();
            alert.AddButton("Invite");
            alert.AddButton("Cancel");
            alert.MessageText = "Invite people";
            alert.InformativeText = "Enter user name";

            var txt = new NSTextField(new CGRect(0, 0, 200, 24));
            alert.AccessoryView = txt;

            var response = alert.RunSheetModal(this.View.Window);
            if(response == (long)NSAlertButtonReturn.First)
            {
                var userName = txt.StringValue;

                var signal = new SignalMessage
                {
                    RtcPeerId = _localId,
                    RtmUserName = RtmService.Instance.UserName,
                    Action = SignalActionTypes.IncomingCall,
                    Data = Channel
                };
                var sugnalText = JsonConvert.SerializeObject(signal);
                RtmService.Instance.SendPeerMessage(userName, sugnalText);
            }
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
            RtmService.Instance.SendChannelMessage(text);
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
            var dataSource = new RemoteVideosTableDataSource(_users);
            RemoteUsersTableView.DataSource = dataSource;
            RemoteUsersTableView.Delegate = new RemoteVideosTableDelegate(dataSource, _agoraKit);

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
                    roomNameLabel.StringValue = $"Room name: {Channel}";
                });
            }
        }

        private void OmMessageReceived(SignalMessage signal)
        {
            var userItemIndex = _users.IndexOf(_users.First(i => i.Uid == signal.RtcPeerId));
            switch (signal.Action)
            {
                case SignalActionTypes.HandDown:
                case SignalActionTypes.HandUp:
                    _users[userItemIndex].IsHandUp = signal.Action == SignalActionTypes.HandUp;
                    RemoteUsersTableView.ReloadData();
                    break;
                case SignalActionTypes.IncomingCall:
                    var alert = new NSAlert();
                    alert.AddButton("Join");
                    alert.AddButton("Cancel");
                    alert.MessageText = "Invite";
                    alert.InformativeText = $"You got invite to [{signal.Data}] room";

                    var response = alert.RunSheetModal(this.View.Window);
                    if (response == (long)NSAlertButtonReturn.First)
                    {
                        LeaveChannel();
                        InitializeAgoraEngine();
                        SetupVideo();
                        ResetButtons();
                        SetupLocalVideo();
                        JoinChannel();
                        InitAndJoinRtm();
                    }
                    break;
            }
        }

        public void LeaveChannel()
        {
            _agoraKit.LeaveChannel(null);
            _agoraKit.SetupLocalVideo(null);
            AgoraRtcEngineKit.Destroy();
            RtmService.Instance.LeaveChannel();
            _users.Clear();
            _agoraKit = null;
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
