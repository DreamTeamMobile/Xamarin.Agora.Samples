using System;
using Android;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Widget;
using DT.Samples.Agora.Shared;
using DT.Xamarin.Agora;

namespace DT.Samples.Agora.Voice.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private const int PERMISSION_REQ_ID_RECORD_AUDIO = 22;

        private RtcEngine _rtcEngine;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            var muteButton = FindViewById<ImageView>(Resource.Id.mute_btn);
            muteButton.Click += (s, e) => OnMuteClicked(muteButton);

            var switchButton = FindViewById<ImageView>(Resource.Id.switch_speaker_btn);
            switchButton.Click += (s, e) => OnSwitchSpeakerClicked(switchButton);

            var endCallButton = FindViewById<ImageView>(Resource.Id.end_call_btn);
            endCallButton.Click += (s, e) => OnEndCallClicked();

            if (CheckSelfPermission(Manifest.Permission.RecordAudio, PERMISSION_REQ_ID_RECORD_AUDIO))
            {
                InitAgoraEngineAndJoinChannel();
            }
        }

        private void InitAgoraEngineAndJoinChannel()
        {
            InitializeAgoraEngine();
            JoinChannel();
        }

        private void InitializeAgoraEngine()
        {
            try
            {
                var handler = new RtcEngineEventHandler();
                handler.DidJoinChannel += () => RunOnUiThread(() => Toast.MakeText(this, "Joined to channel", ToastLength.Short).Show());
                handler.OnRemoteUserJoined += (uid) => RunOnUiThread(() => Toast.MakeText(this, $"User {uid} has joined", ToastLength.Short).Show());
                handler.OnRemoteUserLeft += (uid) => RunOnUiThread(() => Toast.MakeText(this, $"User {uid} has left", ToastLength.Short).Show());
                handler.OnRemoteUserVoiceMuted += (uid, muted) => RunOnUiThread(() => Toast.MakeText(this, $"User {uid} is {(muted ? "muted" : "unmuted" )}", ToastLength.Short).Show());
                _rtcEngine = RtcEngine.Create(this, AgoraTestConstants.AgoraAPI, handler);
                _rtcEngine.SetChannelProfile(Constants.ChannelProfileCommunication);
            }
            catch (Exception)
            {
                Toast.MakeText(this, "Colud not create RtcEngine", ToastLength.Short).Show();
            }
        }

        private void JoinChannel()
        {
            var accessToken = AgoraTestConstants.Token;
            if (string.IsNullOrEmpty(accessToken))
            {
                accessToken = null; // default, no token
            }

            _rtcEngine.JoinChannel(accessToken, "voiceDemoChannel1", string.Empty, 0);
        }

        private void LeaveChannel()
        {
            _rtcEngine.LeaveChannel();
        }

        public bool CheckSelfPermission(string permission, int requestCode)
        {
            if (ContextCompat.CheckSelfPermission(this, permission) != Android.Content.PM.Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new [] { permission }, requestCode);
                return false;
            }
            return true;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            switch (requestCode)
            {
                case PERMISSION_REQ_ID_RECORD_AUDIO:
                    {
                        if (grantResults.Length > 0
                                && grantResults[0] == Android.Content.PM.Permission.Granted)
                        {
                            InitAgoraEngineAndJoinChannel();
                        }
                        else
                        {
                            Toast.MakeText(this, "No permission for " + Manifest.Permission.RecordAudio, ToastLength.Long).Show();
                            Finish();
                        }
                        break;
                    }
            }
        }

        private void OnMuteClicked(ImageView button)
        {
            if (button.Selected)
            {
                button.Selected = false;
                button.ClearColorFilter();
            }
            else
            {
                button.Selected = true;
                button.SetColorFilter(Resources.GetColor(Resource.Color.colorPrimary), PorterDuff.Mode.Multiply);
            }

            _rtcEngine.MuteLocalAudioStream(button.Selected);
        }

        private void OnSwitchSpeakerClicked(ImageView button)
        {
            if (button.Selected)
            {
                button.Selected = false;
                button.ClearColorFilter();
            }
            else
            {
                button.Selected = true;
                button.SetColorFilter(Resources.GetColor(Resource.Color.colorPrimary), PorterDuff.Mode.Multiply);
            }

            _rtcEngine.SetEnableSpeakerphone(button.Selected);
        }

        private void OnEndCallClicked()
        {
            LeaveChannel();
            Finish();
        }
    }
}
