using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;

namespace DT.Samples.Agora.Conference.Droid
{
    [Activity(Label = "@string/app_name", ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginActivity : AppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);
        }

        protected override void OnResume()
        {
            base.OnResume();
            RtmService.Instance.OnLogin += DidLogin;
        }

        protected override void OnPause()
        {
            base.OnPause();
            RtmService.Instance.OnLogin -= DidLogin;
        }

        [Java.Interop.Export("OnLogin")]
        public async void OnLogin(View v)
        {
            var userName = FindViewById<EditText>(Resource.Id.userName).Text;
            if (string.IsNullOrEmpty(userName) || userName.Length > 64)
            {
                Toast.MakeText(this, GetString(Resource.String.invalid_user_name), ToastLength.Short).Show();
            }
            else
            {
                try
                {
                    FindViewById<Button>(Resource.Id.loginButton).Enabled = false;
                    FindViewById<Button>(Resource.Id.loginButton).Text = GetString(Resource.String.loading_button);
                    await RtmService.Instance.Init(this, userName);
                }
                catch(Exception e)
                {
                    FindViewById<Button>(Resource.Id.loginButton).Enabled = true;
                    Toast.MakeText(this, e.Message, ToastLength.Short).Show();
                }
            }
        }

        private void DidLogin(bool success)
        {
            RunOnUiThread(() =>
            {
                if (success)
                {
                    StartActivity(typeof(JoinActivity));
                    FindViewById<Button>(Resource.Id.loginButton).Enabled = true;
                    FindViewById<Button>(Resource.Id.loginButton).Text = GetString(Resource.String.login_button);
                }
                else
                {
                    Toast.MakeText(this, Resource.String.rtm_connect_error, ToastLength.Short).Show();
                }
            });
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
    }
}
