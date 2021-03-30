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
using DT.Xamarin.Agora.Rtm;
using DT.Xamarin.Agora.Video;
using Newtonsoft.Json;

namespace DT.Samples.Agora.Conference.Droid
{
    [Activity(Label = "Room", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/DT.Theme.Room")]
    public class RoomActivity : Activity
    {
        private const int MaxLocalVideoDimension = 150;
        private RtcEngine _agoraEngine;
        private RtmClient _rtmClient;
        private RtmChannel _rtmChannel;
        private AgoraRtmChannelListener _channelListener;
        private AgoraRtcHandler _agoraRtcHandler;
        private AgoraRtmHandler _agoraRtmHandler;
        private bool _isVideoEnabled = true;
        private SurfaceView _localVideoView;
        private uint _localId = 0;
        private ProgressBar _progressBar;
        private ResultCallback _sendMessageChannelCallback;

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

        private async Task InitAndRunRTM()
        {
            _agoraRtmHandler = new AgoraRtmHandler(this);
            _rtmClient = RtmClient.CreateInstance(this, AgoraTestConstants.AgoraAPI, _agoraRtmHandler);

            _sendMessageChannelCallback = new ResultCallback();

            _channelListener = new AgoraRtmChannelListener(this);
            _rtmChannel = _rtmClient.CreateChannel(AgoraSettings.Current.RoomName, _channelListener);

            var channelJoinCallBack = new ResultCallback();
            channelJoinCallBack.OnSuccessAction += (obj) =>
            {
                if (_rtmChannel == null)
                    return;
                RunOnUiThread(() => FindViewById<ImageView>(Resource.Id.hand_up_button).Visibility = ViewStates.Visible);
            };
            channelJoinCallBack.OnFailureAction += (err) =>
            {
                if (_rtmChannel == null)
                    return;
                Toast.MakeText(this, "Could not join to rtm channel", ToastLength.Short).Show();
            };

            var userId = _localId.ToString();
            var rtmToken = await AgoraTokenService.GetRtmToken(userId);
            var loginCallback = new ResultCallback();
            loginCallback.OnSuccessAction += (obj) =>
            {
                _rtmChannel?.Join(channelJoinCallBack);
            };
            loginCallback.OnFailureAction += (err) =>
            {
                if (_rtmChannel == null)
                    return;
                RunOnUiThread(() => Toast.MakeText(this, "Could not RTM login", ToastLength.Short).Show());
            };
            _rtmClient.Login(rtmToken, userId, loginCallback);
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
                if ((int)child.Tag == uid)
                {
                    return child;
                }
            }
            return null;
        }

        private void LeaveChannel()
        {
            Console.WriteLine("LeaveChannel");
            _agoraEngine.LeaveChannel();
            if (_rtmChannel != null)
            {
                _rtmChannel.Leave(null);
                _rtmChannel.Release();
                _rtmChannel = null;
            }
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
            ImageView iv = (ImageView)view;
            if (iv.Selected)
            {
                iv.Selected = false;
                iv.SetImageResource(Resource.Drawable.ic_cam_active_call);
            }
            else
            {
                iv.Selected = true;
                iv.SetImageResource(Resource.Drawable.ic_cam_disabled_call);
            }
            _agoraEngine.MuteLocalVideoStream(iv.Selected);
            _isVideoEnabled = !iv.Selected;
            FindViewById(Resource.Id.local_video_container).Visibility = _isVideoEnabled ? ViewStates.Visible : ViewStates.Gone;
            _localVideoView.Visibility = _isVideoEnabled ? ViewStates.Visible : ViewStates.Gone;
        }

        [Java.Interop.Export("OnLocalAudioMuteClicked")]
        public void OnLocalAudioMuteClicked(View view)
        {
            ImageView iv = (ImageView)view;
            if (iv.Selected)
            {
                iv.Selected = false;
                iv.SetImageResource(Resource.Drawable.ic_mic_active_call);
            }
            else
            {
                iv.Selected = true;
                iv.SetImageResource(Resource.Drawable.ic_mic_inactive_call);
            }
            _agoraEngine.MuteLocalAudioStream(iv.Selected);
            var visibleMutedLayers = iv.Selected ? ViewStates.Visible : ViewStates.Invisible;
            FindViewById(Resource.Id.local_video_overlay).Visibility = visibleMutedLayers;
            FindViewById(Resource.Id.local_video_muted).Visibility = visibleMutedLayers;
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
                PeerId = _localId,
                Action = iv.Selected ? SignalActionTypes.HandUp : SignalActionTypes.HandDown
            };

            var message = _rtmClient.CreateMessage();
            message.Text = JsonConvert.SerializeObject(signal);
            _rtmChannel.SendMessage(message, _sendMessageChannelCallback);
        }

        [Java.Interop.Export("OnSwitchCameraClicked")]
        public void OnSwitchCameraClicked(View view)
        {
            _agoraEngine.SwitchCamera();
        }

        [Java.Interop.Export("OnEncCallClicked")]
        public void OnEncCallClicked(View view)
        {
            LeaveChannel();
            Finish();
        }

        #endregion

        #region AgoraRtmHandler

        public void OnSignalReceived(SignalMessage message)
        {
            RunOnUiThread(() =>
            {
                var view = FindRemoteVideoView(message.PeerId);
                if (view != null)
                {
                    switch (message.Action)
                    {
                        case SignalActionTypes.HandUp:
                            view.IsHandUp = true;
                            break;
                        case SignalActionTypes.HandDown:
                            view.IsHandUp = false;
                            break;
                    }
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
                FindViewById(Resource.Id.local_video_container).Visibility = _isVideoEnabled ? ViewStates.Visible : ViewStates.Invisible;
            });
        }

        public void OnJoinChannelSuccess(string channel, int uid, int elapsed)
        {
            RunOnUiThread(() => _progressBar.Visibility = ViewStates.Gone);
            _localId = (uint)uid;
            InitAndRunRTM();
            RefreshDebug();
        }

        #endregion
    }
}

