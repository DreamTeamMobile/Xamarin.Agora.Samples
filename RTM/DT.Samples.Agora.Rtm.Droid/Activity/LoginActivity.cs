using Android.OS;
using Android.Widget;
using DT.Xamarin.Agora.Rtm;
using Android.Content;
using Android.App;
using Android.Content.PM;
using Android.Support.V7.App;

namespace DT.Samples.Agora.Rtm.Droid
{

    [Activity(Label = "Agora RTM Xamarin", Icon = "@mipmap/app_icon", MainLauncher = true,
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
              Theme = "@style/AppTheme")]

    public class LoginActivity : AppCompatActivity
    {
        private TextView _loginBtn;
        private EditText _userIdEditText;
        private string _userId;

        private RtmClient _rtmClient;
        private bool _isInChat;

        private ResultCallback _callbackResult;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ActivityLogin);

            _userIdEditText = FindViewById<EditText>(Resource.Id.user_id);
            _loginBtn = FindViewById<TextView>(Resource.Id.button_login);

            _loginBtn.Click += OnClickLogin;

            MainApplication.ChatManager = new ChatManager(this);
            MainApplication.ChatManager.Init();

            _rtmClient = MainApplication.ChatManager.GetRtmClient();

            _callbackResult = new ResultCallback();
            _callbackResult.OnSuccessAction += OnSuccessAction;
            _callbackResult.OnFailureAction += OnFailureAction;
        }

        private void OnSuccessAction(Java.Lang.Object obj)
        {
            Intent intent = new Intent(this, typeof(SelectionActivity));
            intent.PutExtra(MessageUtil.IntentExtraUserId, _userId);

            StartActivity(intent);
        }

        private void OnFailureAction(ErrorInfo error)
        {
            RunOnUiThread(() =>
            {
                _loginBtn.Enabled = true;
                _isInChat = false;
                ShowToast(GetString(Resource.String.login_failed));
            });
        }

        public void OnClickLogin(object sender, System.EventArgs e)
        {
            _userId = _userIdEditText.Text;
            if (_userId.Equals(""))
            {
                ShowToast(GetString(Resource.String.account_empty));
            }
            else if (_userId.Length > MessageUtil.MaxInputNameLength)
            {
                ShowToast(GetString(Resource.String.account_too_long));

            }
            else if (_userId.StartsWith(" ", System.StringComparison.Ordinal))
            {
                ShowToast(GetString(Resource.String.account_starts_with_space));

            }
            else if (_userId.Equals("null"))
            {
                ShowToast(GetString(Resource.String.account_literal_null));

            }
            else
            {
                _loginBtn.Enabled = false;
                DoLogin();
            }
        }

        private void ShowToast(string text)
        {
            Toast.MakeText(this, text, ToastLength.Short).Show();
        }

        protected override void OnResume()
        {
            base.OnResume();
            _loginBtn.Enabled = true;
            if (_isInChat)
            {
                DoLogout();
            }
        }

        private void DoLogin()
        {
            _isInChat = true;
            _rtmClient.Login(null, _userId, _callbackResult);
        }

        private void DoLogout()
        {
            _rtmClient.Logout(null);
            MessageUtil.CleanMessageListBeanList();
        }
    }
}