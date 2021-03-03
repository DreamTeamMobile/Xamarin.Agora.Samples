using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using DT.Xamarin.Agora.Rtm;

namespace DT.Samples.Agora.Rtm.Droid
{
    [Activity(Label = "SelectionActivity")]
    public class SelectionActivity : AppCompatActivity {

        private const int ChatRequestCode = 1;

        private TextView _titleTextView;
        private TextView _chatButton;
        private TextView _invitationButton;

        private EditText _nameEditText;
        private EditText _invitationNameEditText;

        private ImageView btnBack;

        private bool _isPeerToPeerMode = true; // whether peer to peer mode or channel mode
        private string _targetName;
        private string _userId;

        private RtmClient _rtmClient;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivitySelection);

            _rtmClient = MainApplication.ChatManager.GetRtmClient();
            InitUIAndData();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == ChatRequestCode)
            {
                if ((int)resultCode == MessageUtil.ActivityResultConnAborted)
                {
                    Finish();
                }
            }
        }

        private void InitUIAndData() {

            _userId = Intent.GetStringExtra(MessageUtil.IntentExtraUserId);
            _titleTextView = FindViewById<TextView>(Resource.Id.selection_title);
            _nameEditText = FindViewById<EditText>(Resource.Id.selection_name);
            _chatButton = FindViewById<TextView>(Resource.Id.selection_chat_btn);

            _invitationButton = FindViewById<TextView>(Resource.Id.invitation_btn);
            _invitationNameEditText = FindViewById<EditText>(Resource.Id.invitation_name);

            RadioGroup modeGroup = FindViewById<RadioGroup>(Resource.Id.mode_radio_group);
            btnBack = FindViewById<ImageView>(Resource.Id.back);

            btnBack.Click += OnClickFinish;
            _chatButton.Click += OnClickChat;
            modeGroup.CheckedChange += CheckedChange;

            _invitationButton.Click += InvitationClick;

            MainApplication.ChatManager.OnAcceptInvitation += OnAcceptInvitation;;

            RadioButton peerMode = FindViewById<RadioButton>(Resource.Id.peer_radio_button);
            peerMode.Checked = true;
        }

        private void OnAcceptInvitation(string userName)
        {
            JumpToMessageActivity(userName);
        }

        private void InvitationClick(object sender, System.EventArgs e)
        {
            MainApplication.ChatManager.SendInvitation(_invitationNameEditText.Text);
        }

        private void CheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            switch (e.CheckedId)
            {
                case Resource.Id.peer_radio_button:
                    _isPeerToPeerMode = true;
                    _titleTextView.Text = GetString(Resource.String.title_peer_msg);
                    _chatButton.Text = GetString(Resource.String.btn_chat);
                    _nameEditText.Hint = GetString(Resource.String.hint_friend);
                    break;
                case Resource.Id.selection_tab_channel:
                    _isPeerToPeerMode = false;
                    _titleTextView.Text = GetString(Resource.String.title_channel_message);
                    _chatButton.Text = GetString(Resource.String.btn_join);
                    _nameEditText.Hint = GetString(Resource.String.hint_channel);
                    break;
            }
        }

        public void OnClickChat(object sender, System.EventArgs e)
        {
            _targetName = _nameEditText.Text;
            if (string.IsNullOrEmpty(_targetName))
            {
                ShowToast(GetString(_isPeerToPeerMode ? Resource.String.account_empty : Resource.String.channel_name_empty));

            }
            else if (_targetName.Length >= MessageUtil.MaxInputNameLength)
            {
                ShowToast(GetString(_isPeerToPeerMode ? Resource.String.account_too_long : Resource.String.channel_name_too_long));

            }
            else if (_targetName.StartsWith(" ", System.StringComparison.Ordinal))
            {
                ShowToast(GetString(_isPeerToPeerMode ? Resource.String.account_starts_with_space : Resource.String.channel_name_starts_with_space));

            }
            else if (_targetName.Equals("null"))
            {
                ShowToast(GetString(_isPeerToPeerMode ? Resource.String.account_literal_null : Resource.String.channel_name_literal_null));

            }
            else if (_isPeerToPeerMode && _targetName.Equals(_userId))
            {
                ShowToast(GetString(Resource.String.account_cannot_be_yourself));

            }
            else
            {
                _chatButton.Enabled = false;
                JumpToMessageActivity();
            }
        }

        private void ShowToast(string text)
        {
            Toast.MakeText(this, text, ToastLength.Short).Show();
        }

        private void JumpToMessageActivity(string userName = null) {

            var chatWithUser = userName ?? _targetName;

            Intent intent = new Intent(this, typeof(MessageActivity));
            intent.PutExtra(MessageUtil.IntentExtraIsPeerMode, _isPeerToPeerMode);
            intent.PutExtra(MessageUtil.IntentExtraTargetName, chatWithUser);
            intent.PutExtra(MessageUtil.IntentExtraUserId, _userId);

            StartActivityForResult(intent, ChatRequestCode);
        }

        protected override void OnResume() {
            base.OnResume();
            _chatButton.Enabled = true;
        }

        public void OnClickFinish(object sender, System.EventArgs e)
        { 
            Finish();
        }
    }
}