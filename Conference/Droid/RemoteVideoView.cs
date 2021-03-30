using System;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace DT.Samples.Agora.Conference.Droid
{
    public class RemoteVideoView : RelativeLayout
    {
        private bool _isAudioMuted;
        public bool IsAudioMuted
        {
            get => _isAudioMuted;
            set
            {
                _isAudioMuted = value;
                var overlay = FindViewById<FrameLayout>(Resource.Id.audio_overlay);
                var muteIcon = FindViewById<ImageView>(Resource.Id.audio_muted);
                overlay.Visibility = muteIcon.Visibility = _isAudioMuted ? ViewStates.Visible : ViewStates.Invisible;
            }
        }

        private bool _isVideoMuted;
        public bool IsVideoMuted
        {
            get => _isVideoMuted;
            set
            {
                _isVideoMuted = value;
                var overlay = FindViewById<FrameLayout>(Resource.Id.video_overlay);
                overlay.Visibility = _isVideoMuted ? ViewStates.Visible : ViewStates.Invisible;
            }
        }

        private bool _isHandUp;
        public bool IsHandUp
        {
            get => _isHandUp;
            set
            {
                _isHandUp = value;
                var overlay = FindViewById<ImageView>(Resource.Id.hand_image);
                overlay.Visibility = _isHandUp ? ViewStates.Visible : ViewStates.Invisible;
            }
        }

        public RemoteVideoView(Context context, IAttributeSet attrs, int defStyle): base(context, attrs, defStyle)
        {
            InitView();
        }

        public RemoteVideoView(Context context, IAttributeSet attrs): base(context, attrs)
        {
            InitView();
        }

        public RemoteVideoView(Context context): base(context)
        {
            InitView();
        }

        public void SetSurface(SurfaceView surfaceView)
        {
            var container = (FrameLayout)FindViewById(Resource.Id.video_view_container);
            container.AddView(surfaceView);
        }

        private void InitView()
        {
            Inflate(Context, Resource.Layout.RemoteVideo, this);
        }
    }
}
