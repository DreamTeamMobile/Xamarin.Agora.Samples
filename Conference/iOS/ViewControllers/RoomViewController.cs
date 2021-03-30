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
        private AgoraRtmChannel _rtmChannel;
        private AgoraRtmKit _rtmKit = new AgoraRtmKit(AgoraTestConstants.AgoraAPI, null);

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
                    _agoraKit.MuteLocalAudioStream(value);
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
                    _agoraKit.MuteLocalVideoStream(value);
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
            Join();
        }

        private async void Join()
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
            JoinRtm();
        }

        private async Task JoinRtm()
        {
            var account = _localId.ToString();
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

                        _rtmChannel = _rtmKit.CreateChannelWithId(AgoraSettings.Current.RoomName, channelDelegate);

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
                HandUpButton.Hidden = false;
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
            var userItemIndex = _userList.IndexOf(_userList.First(i => i.Uid == signal.PeerId));
            switch(signal.Action)
            {
                case SignalActionTypes.HandDown:
                    _userList[userItemIndex].IsHandUp = false;
                    break;
                case SignalActionTypes.HandUp:
                    _userList[userItemIndex].IsHandUp = true;
                    break;
            }
            RemoteVideosContainer.ReloadRows(new [] { NSIndexPath.FromRowSection(userItemIndex, 0)}, UITableViewRowAnimation.Automatic);
        }

        partial void OnHandUpButtonClicked(UIButton sender)
        {
            sender.Selected = !sender.Selected;
            var signalMessage = new SignalMessage
            {
                Action = sender.Selected ? SignalActionTypes.HandUp : SignalActionTypes.HandDown,
                PeerId = _localId
            };
            var text = JsonConvert.SerializeObject(signalMessage);
            var rtmMessage = new AgoraRtmMessage(text);
            _rtmChannel.SendMessage(rtmMessage, (state) =>
            {
                Console.WriteLine($"RTM send channel msg state: {state}");
            });
        }

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

        private void RefreshDebug()
        {
            DebugData.Text = $"local: {_localId}";
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

        public void LeaveChannel()
        {
            _agoraKit.LeaveChannel(null);
            NavigationController.NavigationBarHidden = false;
            NavigationController.PopViewController(true);
        }

        partial void EndCallClicked(NSObject sender)
        {
            LeaveChannel();
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

        public async Task TokenPrivilegeWillExpire(AgoraRtcEngineKit engine, string token)
        {
            var newToken = await AgoraTokenService.GetRtcToken(AgoraSettings.Current.RoomName);
            if (!string.IsNullOrEmpty(newToken))
            {
                _agoraKit.RenewToken(newToken);
            }
        }
    }
}
