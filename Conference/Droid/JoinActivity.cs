﻿using System.Linq;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using DT.Samples.Agora.Shared;
using DT.Samples.Agora.Shared.Helpers;
using DT.Xamarin.Agora;
using Newtonsoft.Json;

namespace DT.Samples.Agora.Conference.Droid
{
    [Activity(Label = "@string/app_name", ScreenOrientation = ScreenOrientation.Portrait)]
    public class JoinActivity : BaseActivity
    {
        protected const int REQUEST_ID = 0;
        protected string[] REQUEST_PERMISSIONS = new string[] {
            Manifest.Permission.Camera,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.RecordAudio,
            Manifest.Permission.ModifyAudioSettings,
            Manifest.Permission.Internet,
            Manifest.Permission.AccessNetworkState
        };

        protected RtcEngine AgoraEngine;
        protected AgoraQualityHandler AgoraHandler;
        protected const string QualityFormat = "Current Connection - {0}";
        protected const string VersionFormat = " {0}";
        private View _layout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Join);
            _layout = FindViewById(Resource.Id.joinLayout);
            CheckPermissions();
            FindViewById<EditText>(Resource.Id.channelName).Text = AgoraSettings.Current.RoomName;
            FindViewById<EditText>(Resource.Id.encryptionKey).Text = AgoraSettings.Current.EncryptionPhrase;
            FindViewById<TextView>(Resource.Id.userName).Text = RtmService.Instance.UserName;
            AgoraHandler = new AgoraQualityHandler(this);
            AgoraEngine = RtcEngine.Create(BaseContext, AgoraTestConstants.AgoraAPI, AgoraHandler);
            AgoraEngine.EnableWebSdkInteroperability(true);
            AgoraEngine.EnableLastmileTest();
            FindViewById<TextView>(Resource.Id.agora_version_text).Text = string.Format(VersionFormat, RtcEngine.SdkVersion);
        }

        protected override void OnResume()
        {
            base.OnResume();
            RtmService.Instance.OnSignalReceived += OnSignalReceived;
        }

        protected override void OnPause()
        {
            base.OnPause();
            RtmService.Instance.OnSignalReceived -= OnSignalReceived;
        }

        protected async Task<bool> CheckPermissions(bool requestPermissions = true)
        {
            var isGranted = REQUEST_PERMISSIONS.Select(permission => ContextCompat.CheckSelfPermission(this, permission) == (int)Permission.Granted).All(granted => granted);
            if (requestPermissions && !isGranted)
            {
                ActivityCompat.RequestPermissions(this, REQUEST_PERMISSIONS, REQUEST_ID);
            }
            return isGranted;
        }

        [Java.Interop.Export("OnJoin")]
        public void OnJoin(View v)
        {
            AgoraSettings.Current.RoomName = FindViewById<EditText>(Resource.Id.channelName).Text;
            if (string.IsNullOrEmpty(AgoraSettings.Current.RoomName))
            {
                Toast.MakeText(this, "Enter Room name", ToastLength.Short).Show();
            }
            else
            {
                AgoraSettings.Current.EncryptionPhrase = FindViewById<EditText>(Resource.Id.encryptionKey).Text;
                CheckPermissionsAndStartCall();
            }
        }

        private async Task CheckPermissionsAndStartCall()
        {
            if (await CheckPermissions(false))
            {
                StartActivity(typeof(RoomActivity));
            }
            else
            {
                Snackbar.Make(_layout, Resource.String.permissions_not_granted, Snackbar.LengthShort).Show();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            switch (item.ItemId)
            {
                case Resource.Id.menu_settings:
                    StartActivity(typeof(SettingsActivity));
                    break;
                case Resource.Id.menu_share:
                    ShareActivity();
                    break;
            }
            return true;
        }

        private void ShareActivity()
        {
            Intent sendIntent = new Intent();
            sendIntent.SetAction(Intent.ActionSend);
            sendIntent.PutExtra(Intent.ExtraText, AgoraTestConstants.ShareString);
            sendIntent.SetType("text/plain");
            StartActivity(sendIntent);
        }

        internal void OnLastmileQuality(int p0)
        {
            RunOnUiThread(() =>
            {
                var textQuality = FindViewById<TextView>(Resource.Id.quality_text);
                var imageQuality = FindViewById<ImageView>(Resource.Id.quality_image);
                string quality = string.Empty;
                switch (p0)
                {
                    case Constants.QualityExcellent:
                        quality = "Excellent";
                        imageQuality.SetImageResource(Resource.Drawable.ic_connection_5);
                        break;
                    case Constants.QualityGood:
                        quality = "Good";
                        imageQuality.SetImageResource(Resource.Drawable.ic_connection_4);
                        break;
                    case Constants.QualityPoor:
                        quality = "Poor";
                        imageQuality.SetImageResource(Resource.Drawable.ic_connection_3);
                        break;
                    case Constants.QualityBad:
                        quality = "Bad";
                        imageQuality.SetImageResource(Resource.Drawable.ic_connection_2);
                        break;
                    case Constants.QualityVbad:
                        quality = "Very Bad";
                        imageQuality.SetImageResource(Resource.Drawable.ic_connection_1);
                        break;
                    case Constants.QualityDown:
                        quality = "Down";
                        imageQuality.SetImageResource(Resource.Drawable.ic_connection_0);
                        break;
                    default:
                        quality = "Unknown";
                        imageQuality.SetImageResource(Resource.Drawable.ic_connection_0);
                        break;
                }
                textQuality.Text = string.Format(QualityFormat, quality);
            });
        }

        protected override void OnDestroy()
        {
            AgoraEngine.DisableLastmileTest();
            if (AgoraHandler != null)
            {
                AgoraHandler.Dispose();
                AgoraHandler = null;
            }
            if (AgoraEngine != null)
            {
                AgoraEngine.Dispose();
                AgoraEngine = null;
            }
            RtcEngine.Destroy();
            RtmService.Instance.Logout();
            base.OnDestroy();
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (CurrentFocus != null)
            {
                InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(CurrentFocus.WindowToken, 0);
            }
            return base.OnTouchEvent(e);
        }

        private void OnSignalReceived(SignalMessage signal)
        {
            if (signal.Action == SignalActionTypes.IncomingCall)
            {
                RunOnUiThread(() =>
                {
                    ShowAlertDialog(string.Format(GetString(Resource.String.invite_message_mask), signal.Data), GetString(Resource.String.accept_button),
                        () =>
                        {
                            AgoraSettings.Current.RoomName = signal.Data;
                            CheckPermissionsAndStartCall();
                        },
                        () =>
                        {
                            var answerSignal = new SignalMessage
                            {
                                Action = SignalActionTypes.RejectCall,
                                RtmUserName = RtmService.Instance.UserName
                            };
                            RtmService.Instance.SendPeerMessage(signal.RtmUserName, JsonConvert.SerializeObject(answerSignal));
                        }
                    );
                });
            }
        }

    }
}
