using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DT.Samples.Agora.Shared;
using DT.Samples.Agora.Shared.Helpers;
using DT.Xamarin.Agora;
using Foundation;
using Newtonsoft.Json;
using UIKit;

namespace DT.Samples.Agora.Conference.iOS
{
    public partial class RoomViewController : UIViewController
    {
        public AgoraRtcDelegate AgoraDelegate;
        private AgoraRtcEngineKit _agoraKit;
        
        private bool _audioMuted = false;
        private bool _videoMuted = false;
        private List<RemoteVideInfo> _userList = new List<RemoteVideInfo>();

        private uint _localId = 0;

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
                    _agoraKit?.MuteLocalAudioStream(value);
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
                    _agoraKit?.MuteLocalVideoStream(value);
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

            RtmService.Instance.OnSignalReceived += OnSignalReceived;
            RtmService.Instance.OnJoinChannel += OnJoinChannel;

            InitAgoraRtc();
            JoinRtc();
            JoinRtm();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            RtmService.Instance.OnSignalReceived -= OnSignalReceived;
            RtmService.Instance.OnJoinChannel -= OnJoinChannel;
        }

        private void InitAgoraRtc()
        {
            AudioMuted = VideoMuted = false;
            AgoraDelegate = new AgoraRtcDelegate(this);
            _agoraKit = AgoraRtcEngineKit.SharedEngineWithAppIdAndDelegate(AgoraTestConstants.AgoraAPI, AgoraDelegate);
            _agoraKit.SetChannelProfile(ChannelProfile.Communication);
            _agoraKit.EnableVideo();
            RemoteVideosContainer.DataSource = new RemoteVideosTableSource(_userList, _agoraKit);
            //var profile = AgoraSettings.Current.UseMySettings ? AgoraSettings.Current.Profile.ToAgoraRtcVideoProfile() : DT.Xamarin.Agora.VideoProfile.Default;
            //AgoraKit.SetVideoProfile(profile, false);
            AgoraRtcVideoCanvas videoCanvas = new AgoraRtcVideoCanvas();
            videoCanvas.Uid = 0;
            videoCanvas.View = LocalView;
            LocalView.Hidden = false;
            videoCanvas.RenderMode = VideoRenderMode.Adaptive;
            _agoraKit.SetupLocalVideo(videoCanvas);
            if (!string.IsNullOrEmpty(AgoraSettings.Current.EncryptionPhrase))
            {
                _agoraKit.SetEncryptionMode(AgoraSettings.Current.EncryptionType.GetModeString());
                _agoraKit.SetEncryptionSecret(AgoraSettings.Current.EncryptionPhrase);
            }
            _agoraKit.StartPreview();
        }

        private async void JoinRtc()
        {
            LoadingIndicator.Hidden = false;
            var token = await AgoraTokenService.GetRtcToken(AgoraSettings.Current.RoomName);
            if (string.IsNullOrEmpty(token))
            {
                //smth went wrong
                LoadingIndicator.Hidden = true;
            }
            else
            {
                _agoraKit.JoinChannelByToken(token, AgoraSettings.Current.RoomName, null, 0, JoiningCompleted);
            }
        }

        private void JoiningCompleted(NSString channel, nuint uid, nint elapsed)
        {
            LoadingIndicator.Hidden = true;
            _localId = (uint)uid;
            _agoraKit.SetEnableSpeakerphone(true);
            UIApplication.SharedApplication.IdleTimerDisabled = true;
            RefreshDebug();
        }

        private void JoinRtm()
        {
            RtmService.Instance.JoinChannel(AgoraSettings.Current.RoomName);
        }

        private void RefreshDebug()
        {
            DebugData.Text = $"local: {_localId}";
        }     

        private void LeaveChannel()
        {
            _agoraKit.LeaveChannel(null);
            AgoraRtcEngineKit.Destroy();
            RtmService.Instance.LeaveChannel();
            _userList.Clear();
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

        #region UI events

        partial void OnHandUpButtonClicked(UIButton sender)
        {
            sender.Selected = !sender.Selected;
            var signalMessage = new SignalMessage
            {
                Action = sender.Selected ? SignalActionTypes.HandUp : SignalActionTypes.HandDown,
                RtcPeerId = _localId
            };
            var text = JsonConvert.SerializeObject(signalMessage);
            RtmService.Instance.SendChannelMessage(text);
        }

        partial void InviteUserButtonClicked(UIKit.UIButton sender)
        {
            var alertView = new UIAlertView("Invite people", "Enter user name", null, "Cancel", "Invite");
            alertView.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
            alertView.Show();

            alertView.Clicked += (s, e) =>
            {
                if (e.ButtonIndex == 1)
                {
                    var userName = alertView.GetTextField(0).Text;
                    if (string.IsNullOrEmpty(userName))
                        return;

                    var signal = new SignalMessage
                    {
                        RtcPeerId = _localId,
                        RtmUserName = RtmService.Instance.UserName,
                        Action = SignalActionTypes.IncomingCall,
                        Data = AgoraSettings.Current.RoomName
                    };
                    var sugnalText = JsonConvert.SerializeObject(signal);
                    RtmService.Instance.SendPeerMessage(userName, sugnalText);
                }
            };
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
            _agoraKit.SwitchCamera();
        }

        partial void EndCallClicked(NSObject sender)
        {
            LeaveChannel();
            NavigationController.NavigationBarHidden = false;
            NavigationController.PopViewController(true);
        }

        #endregion

        #region RTC events

        public void FirstRemoteVideoDecodedOfUid(AgoraRtcEngineKit engine, nuint uid, CoreGraphics.CGSize size, nint elapsed)
        {
            if (_userList.Any(u => u.Uid == uid))
                return;

            var remoteId = (uint)uid;
            _userList.Add(new RemoteVideInfo { Uid = remoteId });
            RemoteVideosContainer.ReloadData();
            if (ContainerView.Hidden)
            {
                ContainerView.Hidden = false;
            }
            RefreshDebug();
        }

        public void DidOfflineOfUid(AgoraRtcEngineKit engine, nuint uid, UserOfflineReason reason)
        {
            _userList.Remove(_userList.First(i => i.Uid == (uint)uid));
            RemoteVideosContainer.ReloadData();
            ContainerView.Hidden = true;
        }

        public void DidVideoMuted(AgoraRtcEngineKit engine, bool muted, nuint uid)
        {
            var userIndex = _userList.IndexOf(_userList.First(i => i.Uid == uid));
            _userList[userIndex].IsVideoMuted = muted;
            RemoteVideosContainer.ReloadRows(new[] { NSIndexPath.FromRowSection(userIndex, 0) }, UITableViewRowAnimation.Automatic);
        }

        public void DidAudioMuted(AgoraRtcEngineKit engine, bool muted, nuint uid)
        {
            var userIndex = _userList.IndexOf(_userList.First(i => i.Uid == uid));
            _userList[userIndex].IsAudioMuted = muted;
            RemoteVideosContainer.ReloadRows(new[] { NSIndexPath.FromRowSection(userIndex, 0) }, UITableViewRowAnimation.Automatic);
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

        public async Task TokenPrivilegeWillExpire(AgoraRtcEngineKit engine, string token)
        {
            var newToken = await AgoraTokenService.GetRtcToken(AgoraSettings.Current.RoomName);
            if (!string.IsNullOrEmpty(newToken))
            {
                _agoraKit.RenewToken(newToken);
            }
        }

        #endregion

        #region RTM events

        private void OnSignalReceived(SignalMessage signal)
        {
            switch (signal.Action)
            {
                case SignalActionTypes.HandDown:
                case SignalActionTypes.HandUp:
                    var userItemIndex = _userList.IndexOf(_userList.First(i => i.Uid == signal.RtcPeerId));
                    _userList[userItemIndex].IsHandUp = signal.Action == SignalActionTypes.HandUp;
                    RemoteVideosContainer.ReloadRows(new[] { NSIndexPath.FromRowSection(userItemIndex, 0) }, UITableViewRowAnimation.Automatic);
                    break;
                case SignalActionTypes.IncomingCall:
                    var alertView = new UIAlertView("Invite", $"You got invite to [{signal.Data}] room", null, "Cancel", "Join");
                    alertView.Show();

                    alertView.Clicked += (s, e) =>
                    {
                        if (e.ButtonIndex == 1)
                        {
                            LeaveChannel();
                            InitAgoraRtc();
                            JoinRtc();
                            JoinRtm();
                        }
                    };
                    break;
            }
        }

        private void OnJoinChannel(bool success)
        {
            HandUpButton.Hidden = !success;
        }

        #endregion
    }
}
