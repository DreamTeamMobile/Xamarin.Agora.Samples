using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using DT.Samples.Agora.Shared;
using DT.Samples.Agora.Shared.Helpers;
using DT.Xamarin.Agora;
using DT.Xamarin.Agora.MediaIO;
using DT.Xamarin.Agora.Video;
using IO.Agora.Advancedvideo.Externvideosource;

namespace DT.Samples.Agora.ScreenSharing.Droid
{
    [Activity(Label = "Room", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/DT.Theme.Room")]
    public class RoomActivity : Activity
    {
        private static string TAG = "RoomActivity";
        private static int DEFAULT_SHARE_FRAME_RATE = 15;

        protected const int MaxLocalVideoDimension = 150;
        protected AgoraRtcHandler AgoraHandler;
        private bool _isVideoEnabled = true;
        private uint _localId = 0;
        private uint _remoteId = 0;
        private ProgressBar _progressBar;
        private IExternalVideoInputService _service;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Room);
            _progressBar = FindViewById<ProgressBar>(Resource.Id.progress_bar);
            if (AgoraSettings.Current.Role == AgoraRole.Listener)
            {
                FindViewById<ImageView>(Resource.Id.mute_video_button).Visibility = ViewStates.Invisible;
            }
            InitAgoraEngineAndJoinChannel();
            FindViewById<TextView>(Resource.Id.room_name).Text = AgoraSettings.Current.RoomName;
        }

        public void OnFirstRemoteVideoDecoded(int uid, int width, int height, int elapsed)
        {
            if (AgoraSettings.Current.Role == AgoraRole.Listener)
            {
                RunOnUiThread(() =>
                {
                    SetupRemoteVideo(uid);
                });
            }
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
            RunOnUiThread(() => _progressBar.Visibility = ViewStates.Gone);
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

        private void InitAgoraEngineAndJoinChannel()
        {
            InitializeAgoraEngine();
            if (AgoraSettings.Current.Role == AgoraRole.Broadcaster)
            {
                BindVideoService();
            }
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
            IO.Agora.Api.Component.Constant.Engine.MuteLocalVideoStream(iv.Selected);
            _isVideoEnabled = !iv.Selected;
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
            IO.Agora.Api.Component.Constant.Engine.MuteLocalAudioStream(iv.Selected);
        }

        [Java.Interop.Export("OnEncCallClicked")]
        public void OnEncCallClicked(View view)
        {
            IO.Agora.Api.Component.Constant.Engine.StopPreview();
            IO.Agora.Api.Component.Constant.Engine.SetupLocalVideo(null);
            IO.Agora.Api.Component.Constant.Engine.LeaveChannel();
            IO.Agora.Api.Component.Constant.Engine.Dispose();
            IO.Agora.Api.Component.Constant.Engine = null;
            Finish();
        }

        public async Task OnTokenPrivilegeWillExpire(string token)
        {
            var newToken = await AgoraTokenService.GetRtcToken(AgoraSettings.Current.RoomName);
            if (!string.IsNullOrEmpty(token))
            {
                IO.Agora.Api.Component.Constant.Engine.RenewToken(newToken);
            }
        }

        private void InitializeAgoraEngine()
        {
            AgoraHandler = new AgoraRtcHandler(this);
            IO.Agora.Api.Component.Constant.Engine = RtcEngine.Create(BaseContext, AgoraTestConstants.AgoraAPI, AgoraHandler);
            IO.Agora.Api.Component.Constant.Textureview = new TextureView(this);
        }

        private void BindVideoService()
        {
            var intent = new Intent();
            intent.SetClass(this.BaseContext, typeof(ExternalVideoInputService));
            var serviceConnection = new VideoInputServiceConnection(this);
            serviceConnection.ServiceConnected += (s) => _service = s;
            BaseContext.BindService(intent, serviceConnection, Bind.AutoCreate);
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
                IO.Agora.Api.Component.Constant.Engine.SetParameters("{\"che.video.mobile_1080p\":true}");
                IO.Agora.Api.Component.Constant.Engine.SetClientRole(Constants.ClientRoleBroadcaster);

                if (AgoraSettings.Current.Role == AgoraRole.Listener)
                {
                    IO.Agora.Api.Component.Constant.Engine.SetChannelProfile(Constants.ChannelProfileCommunication);
                    IO.Agora.Api.Component.Constant.Engine.MuteLocalVideoStream(true);
                }
                else
                {
                    IO.Agora.Api.Component.Constant.Engine.SetChannelProfile(Constants.ChannelProfileLiveBroadcasting);
                    IO.Agora.Api.Component.Constant.Engine.SetVideoSource(new AgoraDefaultSource());
                }

                IO.Agora.Api.Component.Constant.Engine.EnableVideo();
                IO.Agora.Api.Component.Constant.Engine.JoinChannel(token, AgoraSettings.Current.RoomName, string.Empty, 0); // if you do not specify the uid, we will generate the uid for you
            }
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
            IO.Agora.Api.Component.Constant.Engine.SetupRemoteVideo(new VideoCanvas(surfaceView, VideoCanvas.RenderModeAdaptive, uid));
            surfaceView.Tag = uid; // for mark purpose
            RefreshDebug();
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
            if (surfaceView == null)
                return;
            var tag = surfaceView.Tag;
            if (tag != null && (int)tag == uid)
            {
                surfaceView.Visibility = muted ? ViewStates.Gone : ViewStates.Visible;
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == VideoInputServiceConnection.PROJECTION_REQ_CODE && resultCode == Result.Ok)
            {
                try
                {
                    DisplayMetrics metrics = new DisplayMetrics();
                    WindowManager.DefaultDisplay.GetMetrics(metrics);

                    float percent = 0f;
                    float hp = ((float)metrics.HeightPixels) - 1920f;
                    float wp = ((float)metrics.WidthPixels) - 1080f;

                    if (hp < wp)
                    {
                        percent = (((float)metrics.WidthPixels) - 1080f) / ((float)metrics.WidthPixels);
                    }
                    else
                    {
                        percent = (((float)metrics.HeightPixels) - 1920f) / ((float)metrics.HeightPixels);
                    }
                    metrics.HeightPixels = (int)(((float)metrics.HeightPixels) - (metrics.HeightPixels * percent));
                    metrics.WidthPixels = (int)(((float)metrics.WidthPixels) - (metrics.WidthPixels * percent));

                    data.PutExtra(ExternalVideoInputManager.FLAG_SCREEN_WIDTH, metrics.WidthPixels);
                    data.PutExtra(ExternalVideoInputManager.FLAG_SCREEN_HEIGHT, metrics.HeightPixels);
                    data.PutExtra(ExternalVideoInputManager.FLAG_SCREEN_DPI, (int)metrics.Density);
                    data.PutExtra(ExternalVideoInputManager.FLAG_FRAME_RATE, DEFAULT_SHARE_FRAME_RATE);
                    SetVideoConfig(metrics.WidthPixels, metrics.HeightPixels);
                    _service.SetExternalVideoInput(ExternalVideoInputManager.TYPE_SCREEN_SHARE, data);
                }
                catch (RemoteException e)
                {
                    e.PrintStackTrace();
                }
            }
        }

        private void SetVideoConfig(int width, int height)
        {
            Log.Info(TAG, "SDK encoding ->width:" + width + ",height:" + height);
            /**Setup video stream encoding configs*/
            IO.Agora.Api.Component.Constant.Engine.SetVideoEncoderConfiguration(new VideoEncoderConfiguration(
                    new VideoEncoderConfiguration.VideoDimensions(width, height),
                    VideoEncoderConfiguration.FRAME_RATE.FrameRateFps15,
                    VideoEncoderConfiguration.StandardBitrate,
                    VideoEncoderConfiguration.ORIENTATION_MODE.OrientationModeFixedPortrait
            ));
        }
    }
}

