using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using DT.Samples.Agora.Shared;
using DT.Samples.Agora.Shared.Helpers;
using DT.Xamarin.Agora;
using DT.Xamarin.Agora.Video;
using Newtonsoft.Json;

namespace DT.Samples.Agora.Conference.Droid
{
    [Activity(Label = "Room", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/DT.Theme.Room")]
    public class RoomActivity : BaseActivity
    {
        private const int MaxLocalVideoDimension = 150;
        private RtcEngine _agoraEngine;
        private AgoraRtcHandler _agoraRtcHandler;

        private bool _isVideoEnabled = true;
        private bool IsVideoEnabled
        {
            get => _isVideoEnabled;
            set
            {
                _isVideoEnabled = value;
                var iv = FindViewById<ImageView>(Resource.Id.mute_video_button);
                if (IsVideoEnabled)
                {
                    iv.SetImageResource(Resource.Drawable.ic_cam_active_call);
                }
                else
                {
                    iv.SetImageResource(Resource.Drawable.ic_cam_disabled_call);
                }
                _agoraEngine?.MuteLocalVideoStream(!IsVideoEnabled);
                FindViewById(Resource.Id.local_video_container).Visibility = IsVideoEnabled ? ViewStates.Visible : ViewStates.Gone;
                if(_localVideoView != null)
                    _localVideoView.Visibility = IsVideoEnabled ? ViewStates.Visible : ViewStates.Gone;
            }
        }

        private bool _isAudioEnabled = true;
        private bool IsAudioEnabled
        {
            get => _isAudioEnabled;
            set
            {
                _isAudioEnabled = value;
                var iv = FindViewById<ImageView>(Resource.Id.mute_audio_button);
                if (_isAudioEnabled)
                {
                    iv.SetImageResource(Resource.Drawable.ic_mic_active_call);
                }
                else
                {
                    iv.SetImageResource(Resource.Drawable.ic_mic_inactive_call);
                }
                _agoraEngine?.MuteLocalAudioStream(!IsAudioEnabled);
                var visibleMutedLayers = !IsAudioEnabled ? ViewStates.Visible : ViewStates.Invisible;
                FindViewById(Resource.Id.local_video_overlay).Visibility = visibleMutedLayers;
                FindViewById(Resource.Id.local_video_muted).Visibility = visibleMutedLayers;
            }
        }

        private SurfaceView _localVideoView;
        private uint _localId = 0;
        private ProgressBar _progressBar;
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Room);
            _progressBar = FindViewById<ProgressBar>(Resource.Id.progress_bar);
            InitAgoraEngineAndJoinChannel();
            FindViewById<TextView>(Resource.Id.room_name).Text = AgoraSettings.Current.RoomName;
        }

        private void InitAgoraEngineAndJoinChannel()
        {
            InitializeAgoraEngine();
            SetupVideoProfile();
            SetupLocalVideo();
            JoinChannel();
        }

        private void InitializeAgoraEngine()
        {
            IsVideoEnabled = true;
            IsAudioEnabled = true;
            _agoraRtcHandler = new AgoraRtcHandler(this);
            _agoraEngine = RtcEngine.Create(BaseContext, AgoraTestConstants.AgoraAPI, _agoraRtcHandler);
        }

        private void SetupVideoProfile()
        {
            _agoraEngine.EnableVideo();
            _agoraEngine.SetVideoProfile(AgoraSettings.Current.UseMySettings ? AgoraSettings.Current.Profile : Constants.VideoProfile360p, false);
        }

        private void SetupLocalVideo()
        {
            var container = (FrameLayout)FindViewById(Resource.Id.local_video_view_container);
            _localVideoView = RtcEngine.CreateRendererView(BaseContext);
            _localVideoView.SetZOrderMediaOverlay(true);
            container.AddView(_localVideoView);
            _agoraEngine.SetupLocalVideo(new VideoCanvas(_localVideoView, VideoCanvas.RenderModeAdaptive, 0));
            if (!string.IsNullOrEmpty(AgoraSettings.Current.EncryptionPhrase))
            {
                _agoraEngine.SetEncryptionMode(AgoraSettings.Current.EncryptionType.GetModeString());
                _agoraEngine.SetEncryptionSecret(AgoraSettings.Current.EncryptionPhrase);
            }
            _agoraEngine.StartPreview();
        }

        private async Task JoinChannel()
        {
            _progressBar.Visibility = ViewStates.Visible;
            var token = await AgoraTokenService.GetRtcToken(AgoraSettings.Current.RoomName);
            if (string.IsNullOrEmpty(token))
            {
                _progressBar.Visibility = ViewStates.Gone;
            }
            else
            {
                _agoraEngine.JoinChannel(token, AgoraSettings.Current.RoomName, string.Empty, 0);
            }
        }

        private void RefreshDebug()
        {
            RunOnUiThread(() =>
            {
                FindViewById<TextView>(Resource.Id.debug_data).Text = $"local: {_localId}";
            });
        }

        private void SetupRemoteVideo(int uid)
        {
            var container = (LinearLayout)FindViewById(Resource.Id.remote_videos_container);
            var surfaceView = RtcEngine.CreateRendererView(BaseContext);
            var remoteView = new RemoteVideoView(this);
            remoteView.Tag = uid; // for mark purpose
            remoteView.SetSurface(surfaceView);
            container.AddView(remoteView);
            _agoraEngine.SetupRemoteVideo(new VideoCanvas(surfaceView, VideoCanvas.RenderModeAdaptive, uid));
        }

        private void OnRemoteUserLeft(uint uid)
        {
            var container = (LinearLayout)FindViewById(Resource.Id.remote_videos_container);
            var view = FindRemoteVideoView(uid);
            if (view != null)
            {
                container.RemoveView(view);
            }
        }

        private void OnRemoteUserAudioMuted(uint uid, bool muted)
        {
            var view = FindRemoteVideoView(uid);
            if (view != null)
            {
                view.IsAudioMuted = muted;
            }
        }

        private void OnRemoteUserVideoMuted(uint uid, bool muted)
        {
            var view = FindRemoteVideoView(uid);
            if(view != null)
            {
                view.IsVideoMuted = muted;
            }
        }

        private RemoteVideoView FindRemoteVideoView(uint uid)
        {
            var container = (LinearLayout)FindViewById(Resource.Id.remote_videos_container);
            for (int i = 0; i < container.ChildCount; i++)
            {
                var child = container.GetChildAt(i) as RemoteVideoView;
                if ((int)child.Tag == (int)uid)
                {
                    return child;
                }
            }
            return null;
        }

        private void LeaveChannel()
        {
            Console.WriteLine("LeaveChannel");
            var localContainer = (FrameLayout)FindViewById(Resource.Id.local_video_view_container);
            localContainer.RemoveAllViews();
            var remoteContainer = (LinearLayout)FindViewById(Resource.Id.remote_videos_container);
            remoteContainer.RemoveAllViews();

            _agoraEngine.LeaveChannel();
            RtmService.Instance.OnJoinChannel -= DidJionChannel;
            RtmService.Instance.OnSignalReceived -= OnSignalReceived;
            RtmService.Instance.LeaveChannel();
            RtcEngine.Destroy();
        }

        protected override void OnDestroy()
        {
            LeaveChannel();
            base.OnDestroy();
        }

        #region Button Events

        [Java.Interop.Export("OnLocalVideoMuteClicked")]
        public void OnLocalVideoMuteClicked(View view)
        {
            IsVideoEnabled = !IsVideoEnabled;
        }

        [Java.Interop.Export("OnLocalAudioMuteClicked")]
        public void OnLocalAudioMuteClicked(View view)
        {
            IsAudioEnabled = !IsAudioEnabled;
        }

        [Java.Interop.Export("OnHanUpClicked")]
        public void OnHanUpClicked(View view)
        {
            var iv = (ImageView)view;
            if (iv.Selected)
            {
                iv.Selected = false;
                iv.SetImageResource(Resource.Drawable.ic_hand);
            }
            else
            {
                iv.Selected = true;
                iv.SetImageResource(Resource.Drawable.ic_hand_selected);
            }
            var signal = new SignalMessage
            {
                RtcPeerId = _localId,
                Action = iv.Selected ? SignalActionTypes.HandUp : SignalActionTypes.HandDown
            };

            var text = JsonConvert.SerializeObject(signal);
            RtmService.Instance.SendChannelMessage(text);
        }

        [Java.Interop.Export("OnSwitchCameraClicked")]
        public void OnSwitchCameraClicked(View view)
        {
            _agoraEngine.SwitchCamera();
        }

        [Java.Interop.Export("OnAddUserClicked")]
        public void OnAddUserClicked(View view)
        {
            ShowEntryAlertDialog(GetString(Resource.String.invite_dialog_title), GetString(Resource.String.invite_user_hint), GetString(Resource.String.invite_button),
                (text) =>
                {
                    var userName = text;
                    var signal = new SignalMessage
                    {
                        RtcPeerId = _localId,
                        RtmUserName = RtmService.Instance.UserName,
                        Action = SignalActionTypes.IncomingCall,
                        Data = AgoraSettings.Current.RoomName
                    };
                    var sugnalText = JsonConvert.SerializeObject(signal);
                    RtmService.Instance.SendPeerMessage(userName, sugnalText);
                },
                null);
        }

        [Java.Interop.Export("OnEncCallClicked")]
        public void OnEncCallClicked(View view)
        {
            Finish();
        }

        #endregion

        #region AgoraRtmHandler

        public void OnSignalReceived(SignalMessage message)
        {
            RunOnUiThread(() =>
            {
                switch (message.Action)
                {
                    case SignalActionTypes.HandUp:
                    case SignalActionTypes.HandDown:
                        var view = FindRemoteVideoView(message.RtcPeerId);
                        if (view != null)
                        {
                            view.IsHandUp = message.Action == SignalActionTypes.HandUp;
                        }
                        break;
                    case SignalActionTypes.IncomingCall:
                        ShowAlertDialog(string.Format(GetString(Resource.String.invite_message_mask), message.Data), GetString(Resource.String.accept_button),
                            () =>
                            {
                                AgoraSettings.Current.RoomName = message.Data;
                                LeaveChannel();
                                InitAgoraEngineAndJoinChannel();
                            },
                            () =>
                            {
                                var answerSignal = new SignalMessage
                                {
                                    Action = SignalActionTypes.RejectCall,
                                    RtmUserName = RtmService.Instance.UserName
                                };
                                RtmService.Instance.SendPeerMessage(message.RtmUserName, JsonConvert.SerializeObject(answerSignal));
                            }
                        );
                        break;
                }
            });
        }

        #endregion

        #region AgoraRtcHandler

        public void OnFirstRemoteVideoDecoded(int uid, int width, int height, int elapsed)
        {
            RunOnUiThread(() =>
            {
                SetupRemoteVideo(uid);
            });
        }

        public void OnUserOffline(uint uid, int reason)
        {
            RunOnUiThread(() =>
            {
                OnRemoteUserLeft(uid);
            });
        }

        public void OnUserMuteAudio(uint uid, bool muted)
        {
            RunOnUiThread(() =>
            {
                OnRemoteUserAudioMuted(uid, muted);
            });
        }

        public void OnUserMuteVideo(uint uid, bool muted)
        {
            RunOnUiThread(() =>
            {
                OnRemoteUserVideoMuted(uid, muted);
            });
        }

        public void OnError(int errorCode)
        {
            RunOnUiThread(() => Toast.MakeText(this, $"Smth went wrong. Error code: {errorCode}", ToastLength.Short).Show());
        }

        public async Task OnTokenPrivilegeWillExpire(string token)
        {
            var newToken = await AgoraTokenService.GetRtcToken(AgoraSettings.Current.RoomName);
            if (!string.IsNullOrEmpty(token))
            {
                _agoraEngine.RenewToken(newToken);
            }
        }

        public void OnFirstLocalVideoFrame(float height, float width, int p2)
        {
            var ratio = height / width;
            var ratioHeight = ratio * MaxLocalVideoDimension;
            var ratioWidth = MaxLocalVideoDimension / ratio;
            var containerHeight = height > width ? MaxLocalVideoDimension : ratioHeight;
            var containerWidth = height > width ? ratioWidth : MaxLocalVideoDimension;
            RunOnUiThread(() =>
            {
                var videoContainer = FindViewById<RelativeLayout>(Resource.Id.local_video_container);
                var parameters = videoContainer.LayoutParameters;
                parameters.Height = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, containerHeight, Resources.DisplayMetrics);
                parameters.Width = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, containerWidth, Resources.DisplayMetrics);
                videoContainer.LayoutParameters = parameters;
                FindViewById(Resource.Id.local_video_container).Visibility = IsVideoEnabled ? ViewStates.Visible : ViewStates.Invisible;
            });
        }

        public void OnJoinChannelSuccess(string channel, int uid, int elapsed)
        {
            RunOnUiThread(() => _progressBar.Visibility = ViewStates.Gone);
            _localId = (uint)uid;
            RtmService.Instance.OnJoinChannel += DidJionChannel;
            RtmService.Instance.OnSignalReceived += OnSignalReceived;
            RtmService.Instance.JoinChannel(channel); 
            RefreshDebug();
        }

        private void DidJionChannel(bool success)
        {
            if (success)
            {
                RunOnUiThread(() => FindViewById<ImageView>(Resource.Id.hand_up_button).Visibility = ViewStates.Visible);
            }
        }

        #endregion
    }
}

