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

namespace DT.Samples.Agora.OneToOne.Droid
{
    [Activity(Label = "Room", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/DT.Theme.Room")]
    public class RoomActivity : Activity
    {
        protected const int MaxLocalVideoDimension = 150;
        protected RtcEngine AgoraEngine;
        protected AgoraRtcHandler AgoraHandler;
        private bool _isVideoEnabled = true;
        private SurfaceView _localVideoView;
        private uint _localId = 0;
        private uint _remoteId = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Room);
            InitAgoraEngineAndJoinChannel();
            FindViewById<TextView>(Resource.Id.room_name).Text = AgoraSettings.Current.RoomName;
        }

        public void OnFirstRemoteVideoDecoded(int uid, int width, int height, int elapsed)
        {
            RunOnUiThread(() =>
            {
                SetupRemoteVideo(uid);
            });
        }

        public void OnUserOffline(int uid, int reason)
        {
            RunOnUiThread(() =>
            {
                OnRemoteUserLeft();
            });
        }

        public void OnUserMuteVideo(int uid, bool muted)
        {
            RunOnUiThread(() =>
            {
                OnRemoteUserVideoMuted(uid, muted);
            });
        }

        public void OnJoinChannelSuccess(string channel, int uid, int elapsed)
        {
            _localId = (uint)uid;
            RefreshDebug();
        }

        public void RefreshDebug()
        {
            RunOnUiThread(() =>
            {
                FindViewById<TextView>(Resource.Id.debug_data).Text = $"local: {_localId}\nremote: {_remoteId}";
            });
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

        private void InitAgoraEngineAndJoinChannel()
        {
            InitializeAgoraEngine();
            SetupVideoProfile();
            SetupLocalVideo();
            JoinChannel();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

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
            AgoraEngine.MuteLocalVideoStream(iv.Selected);
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
            AgoraEngine.MuteLocalAudioStream(iv.Selected);
            var visibleMutedLayers = iv.Selected ? ViewStates.Visible : ViewStates.Invisible;
            FindViewById(Resource.Id.local_video_overlay).Visibility = visibleMutedLayers;
            FindViewById(Resource.Id.local_video_muted).Visibility = visibleMutedLayers;
        }

        [Java.Interop.Export("OnSwitchCameraClicked")]
        public void OnSwitchCameraClicked(View view)
        {
            AgoraEngine.SwitchCamera();
        }

        [Java.Interop.Export("OnEncCallClicked")]
        public void OnEncCallClicked(View view)
        {
            AgoraEngine.StopPreview();
            AgoraEngine.SetupLocalVideo(null);
            AgoraEngine.LeaveChannel();
            AgoraEngine.Dispose();
            AgoraEngine = null;
            Finish();
        }

        private void InitializeAgoraEngine()
        {
            AgoraHandler = new AgoraRtcHandler(this);
            AgoraEngine = RtcEngine.Create(BaseContext, AgoraTestConstants.AgoraAPI, AgoraHandler);
        }

        private void SetupVideoProfile()
        {
            AgoraEngine.EnableVideo();
            AgoraEngine.SetVideoProfile(AgoraSettings.Current.UseMySettings ? AgoraSettings.Current.Profile : Constants.VideoProfile360p, false);
        }

        private void SetupLocalVideo()
        {
            FrameLayout container = (FrameLayout)FindViewById(Resource.Id.local_video_view_container);
            _localVideoView = RtcEngine.CreateRendererView(BaseContext);
            _localVideoView.SetZOrderMediaOverlay(true);
            container.AddView(_localVideoView);
            AgoraEngine.SetupLocalVideo(new VideoCanvas(_localVideoView, VideoCanvas.RenderModeAdaptive, 0));
            if (!string.IsNullOrEmpty(AgoraSettings.Current.EncryptionPhrase))
            {
                AgoraEngine.SetEncryptionMode(AgoraSettings.Current.EncryptionType.GetModeString());
                AgoraEngine.SetEncryptionSecret(AgoraSettings.Current.EncryptionPhrase);
            }
            AgoraEngine.StartPreview();
        }

        private void JoinChannel()
        {
            AgoraEngine.JoinChannel(null, AgoraSettings.Current.RoomName, string.Empty, 0); // if you do not specify the uid, we will generate the uid for you
        }

        private void SetupRemoteVideo(int uid)
        {
            _remoteId = (uint)uid;
            FrameLayout container = (FrameLayout)FindViewById(Resource.Id.remote_video_view_container);
            if (container.ChildCount >= 1)
            {
                return;
            }
            SurfaceView surfaceView = RtcEngine.CreateRendererView(BaseContext);
            container.AddView(surfaceView);
            AgoraEngine.SetupRemoteVideo(new VideoCanvas(surfaceView, VideoCanvas.RenderModeAdaptive, uid));
            surfaceView.Tag = uid; // for mark purpose
            RefreshDebug();
        }

        private void LeaveChannel()
        {
            AgoraEngine.LeaveChannel();
        }

        private void OnRemoteUserLeft()
        {
            FrameLayout container = (FrameLayout)FindViewById(Resource.Id.remote_video_view_container);
            container.RemoveAllViews();
        }

        private void OnRemoteUserVideoMuted(int uid, bool muted)
        {
            FrameLayout container = (FrameLayout)FindViewById(Resource.Id.remote_video_view_container);
            SurfaceView surfaceView = (SurfaceView)container.GetChildAt(0);
            var tag = surfaceView.Tag;
            if (tag != null && (int)tag == uid)
            {
                surfaceView.Visibility = muted ? ViewStates.Gone : ViewStates.Visible;
            }
        }
    }
}

