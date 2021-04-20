using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Widget;
using DT.Xamarin.Agora.Rtm;
using Android.Runtime;
using Android.Content;
using Android.Provider;
using Android.Support.V4.App;
using Android;
using Android.Support.V4.Content;
using Android.Content.PM;
using DT.Samples.Agora.Rtm.Droid.Utils;

namespace DT.Samples.Agora.Rtm.Droid
{
    [Activity(Label = "MessageActivity", Theme = "@style/AppTheme")]
    public class MessageActivity : AppCompatActivity 
    {
        private const int ReadExternalStoragePermissionRequestCode = 1;

        private TextView _titleTextView;
        private TextView _btnSend;
        private ImageView _btnImageSend;
        private EditText _msgEditText;
        private ImageView _btnBack;
        private RecyclerView _recyclerView;
        private List<MessageBean> _messageBeanList = new List<MessageBean>();
        private MessageAdapter _messageAdapter;

        private bool _isPeerToPeerMode = true;
        private string _userId = "";
        private string _peerId = "";
        private string _channelName = "";
        private int _channelMemberCount = 1;

        private ChatManager _chatManager;
        private RtmClient rtmClient;
        private RtmClientListener _myRtmClientListener;
        private RtmChannel _rtmChannel;

        private ResultCallback _channelJoinCallBack;
        private ResultCallback _memberCallBack;

        private ResultCallback _sendMessageChannelCallback;
        private ResultCallback _sendMessageClientCallback;

        private RtmChannelListener _channelListener;

        private bool CanReadExternalStorage => ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) == (int)Permission.Granted;

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityMessage);

            InitUIAndData();
            InitChat();
        }

        private void OnMessageReceived(RtmMessage message, RtmChannelMember fromMember)
        {
            var account = fromMember.UserId;
            MessageBean messageBean = new MessageBean(account, message, false)
            {
                Background = GetMessageColor(account)
            };

            RunOnUiThread(() =>
            {
                _messageBeanList.Add(messageBean);
                _messageAdapter.NotifyItemRangeChanged(_messageBeanList.Count, 1);
                _recyclerView.ScrollToPosition(_messageBeanList.Count - 1);
            });
        }

        private void OnMemberJoined(RtmChannelMember obj)
        {
            _channelMemberCount++;
            RefreshChannelTitle();
        }

        private void OnMemberLeft(RtmChannelMember obj)
        {
            _channelMemberCount--;
            RefreshChannelTitle();
        }

        private void OnRtmMessageReceived(RtmMessage message, string peerId)
        {
            RunOnUiThread(() =>
            {
                if (_peerId.Equals(peerId))
                {
                    MessageBean messageBean = new MessageBean(peerId, message, false)
                    {
                        Background = GetMessageColor(peerId)
                    };

                    _messageBeanList.Add(messageBean);
                    _messageAdapter.NotifyItemRangeChanged(_messageBeanList.Count, 1);
                    _recyclerView.ScrollToPosition(_messageBeanList.Count - 1);
                }
                else
                {
                    MessageUtil.AddMessageBean(peerId, message);
                }
            });
        }

        private void OnStateChanged(int state, int reason)
        {
            switch (state)
            {
                case RtmStatusCodeConnectionState.ConnectionStateReconnecting:
                    ShowToast(GetString(Resource.String.reconnecting));
                    break;
                case RtmStatusCodeConnectionState.ConnectionStateAborted:
                    ShowToast(GetString(Resource.String.account_offline));
                    SetResult((Result)MessageUtil.ActivityResultConnAborted);
                    Finish();
                    break;
            }
        }

        private void Member_OnFailureAction()
        {
            ShowToast(GetString(Resource.String.join_channel_failed));
            Finish();
        }

        private void ShowToast(string text)
        {
            RunOnUiThread(() =>
            {
                Toast.MakeText(this, text, ToastLength.Short).Show();
            });
        }

        private void InitUIAndData()
        {
            InitCallbackAndListener();
            _chatManager = MainApplication.ChatManager;
            _chatManager.RegisterListener(_myRtmClientListener);

            _titleTextView = FindViewById<TextView>(Resource.Id.message_title);
            _msgEditText = FindViewById<EditText>(Resource.Id.message_edittiext);
            _recyclerView = FindViewById<RecyclerView>(Resource.Id.message_list);
            _btnSend = FindViewById<TextView>(Resource.Id.selection_chat_btn);
            _btnImageSend = FindViewById<ImageView>(Resource.Id.selection_img_btn);
            _btnBack = FindViewById<ImageView>(Resource.Id.back);

            _btnBack.Click += OnClickFinish;
            _btnSend.Click += OnClickTextSend;
            _btnImageSend.Click += OnClickImageSend;

            _isPeerToPeerMode = Intent.GetBooleanExtra(MessageUtil.IntentExtraIsPeerMode, true);
            _userId = Intent.GetStringExtra(MessageUtil.IntentExtraUserId);
            string targetName = Intent.GetStringExtra(MessageUtil.IntentExtraTargetName);
            if (_isPeerToPeerMode) {
                _peerId = targetName;
                _titleTextView.Text = _peerId;
                MessageListBean messageListBean = MessageUtil.GetExistMessageListBean(_peerId);
                if (messageListBean != null) {
                    _messageBeanList.AddRange(messageListBean.MessageBeanList);
                }
                MessageListBean offlineMessageBean = new MessageListBean(_peerId, _chatManager);
                _messageBeanList.AddRange(offlineMessageBean.MessageBeanList);
                _chatManager.RemoveAllOfflineMessages(_peerId);

                _titleTextView.Text = $"Chat with {_peerId}";
            } else {
                _channelName = targetName;
                _channelMemberCount = 1;
                _titleTextView.Text = $"{_channelName}({_channelMemberCount})";
            }

            LinearLayoutManager layoutManager = new LinearLayoutManager(this);
            _recyclerView.SetLayoutManager(layoutManager);
            layoutManager.Orientation = OrientationHelper.Vertical;
            _messageAdapter = new MessageAdapter(this, _messageBeanList);
            _recyclerView.SetAdapter(_messageAdapter);

        }

        private void InitChat()
        {
            rtmClient = _chatManager.GetRtmClient();

            if (!_isPeerToPeerMode)
            {
                CreateAndJoinChannel();
            }
            else
            {
                JoinPeer();
            }
        }

        private void InitCallbackAndListener()
        {
            _channelJoinCallBack = new ResultCallback();
            _channelJoinCallBack.OnSuccessAction += OnSuccessJoinChannel;
            _channelJoinCallBack.OnFailureAction += OnFailureJoinChannel;

            _sendMessageClientCallback = new ResultCallback();
            _sendMessageClientCallback.OnFailureAction += OnFailureSendToClient;

            _sendMessageChannelCallback = new ResultCallback();
            _sendMessageChannelCallback.OnFailureAction += OnFailureSendChannel;

            _memberCallBack = new ResultCallback();
            _memberCallBack.OnSuccessAction += MemberSuccessAction;

            _myRtmClientListener = new RtmClientListener();
            _myRtmClientListener.MessageReceived += OnRtmMessageReceived;
            _myRtmClientListener.StateChanged += OnStateChanged;

            _channelListener = new RtmChannelListener();
            _channelListener.OnMemberLeftAction += OnMemberLeft;
            _channelListener.OnMemberJoinedAction += OnMemberJoined;
            _channelListener.OnMessageReceivedAction += OnMessageReceived;
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            if (_isPeerToPeerMode) {
                MessageUtil.AddMessageListBeanList(new MessageListBean(_peerId, _messageBeanList));
            } else {
                LeaveAndReleaseChannel();
            }
            _chatManager.UnregisterListener(_myRtmClientListener);
        }

        private void OnFailureSendChannel(ErrorInfo error)
        {
            switch (error.ErrorCode)
            {
                case RtmStatusCodeChannelMessageError.ChannelMessageErrTimeout:
                case RtmStatusCodeChannelMessageError.ChannelMessageErrFailure:
                    ShowToast(GetString(Resource.String.send_msg_failed));
                    break;
            }
        }

        private void OnFailureJoinChannel(ErrorInfo error)
        {
            ShowToast(GetString(Resource.String.join_channel_failed));
            Finish();
        }

        private void OnSuccessJoinChannel(Java.Lang.Object obj)
        {
            GetChannelMemberList();
        }

        private void OnFailureSendToClient(ErrorInfo error)
        {
            switch (error.ErrorCode)
            {
                case RtmStatusCodePeerMessageError.PeerMessageErrTimeout:
                case RtmStatusCodePeerMessageError.PeerMessageErrFailure:
                    ShowToast(GetString(Resource.String.send_msg_failed));
                    break;
                case RtmStatusCodePeerMessageError.PeerMessageErrPeerUnreachable:
                    ShowToast(GetString(Resource.String.peer_offline));
                    break;
                case RtmStatusCodePeerMessageError.PeerMessageErrInvalidMessage:
                    ShowToast(GetString(Resource.String.send_msg_failed_invalid));
                    RunOnUiThread(() =>
                    {
                        _messageBeanList.RemoveAt(_messageBeanList.Count - 1);
                        _messageAdapter.NotifyItemRangeChanged(_messageBeanList.Count, 1);
                        _recyclerView.ScrollToPosition(_messageBeanList.Count - 1);
                    });
                    break;
            }
        }

        private void JoinPeer()
        {
            var message = rtmClient.CreateMessage();

            message.Text = $"{_userId} joined";
            MessageBean messageBean = new MessageBean(_userId, message, true);
            _messageBeanList.Add(messageBean);
            _messageAdapter.NotifyItemRangeChanged(_messageBeanList.Count, 1);
            _recyclerView.ScrollToPosition(_messageBeanList.Count - 1);

            SendPeerMessage(message);
        }

        private void MemberSuccessAction(Java.Lang.Object obj)
        {
            if (obj is JavaList propertyInfo)
            {
                List<RtmChannelMember> responseInfo = new List<RtmChannelMember>();

                foreach (var item in propertyInfo)
                {
                    responseInfo.Add((RtmChannelMember)item);
                }

                RunOnUiThread(() =>
                {
                    _channelMemberCount = responseInfo.Count;
                    RefreshChannelTitle();
                });
            }
        }

        public void OnClickTextSend(object sender, EventArgs e)
        {
            string messageText = _msgEditText.Text;
            if (!string.IsNullOrEmpty(messageText)) {
                var message = rtmClient.CreateMessage();
                message.Text = messageText;
                MessageBean messageBean = new MessageBean(_userId, message, true);
                _messageBeanList.Add(messageBean);
                _messageAdapter.NotifyItemRangeChanged(_messageBeanList.Count, 1);
                _recyclerView.ScrollToPosition(_messageBeanList.Count - 1);
                if (_isPeerToPeerMode) {
                    SendPeerMessage(message);
                } else {
                    SendChannelMessage(message);
                }
            }
            _msgEditText.Text = "";
        }

        public void OnClickImageSend(object sender, EventArgs e)
        {
            if (!CanReadExternalStorage)
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadExternalStorage }, ReadExternalStoragePermissionRequestCode);
            }
            else
            {
                GetImageFromGalery();
            }
        }

        private void GetImageFromGalery()
        {
            var intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
            StartActivityForResult(Intent.CreateChooser(intent, "Select image from galery"), 0);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            switch(requestCode)
            {
                case 0:
                    if (data != null)
                    {
                        var resultUri = data.Data;
                        var path = GetPath(resultUri);
                        
                        var resultCallback = new ResultCallback();
                        resultCallback.OnSuccessAction += (message) =>
                        {
                            RunOnUiThread(() =>
                            {
                                var rtmImageMessage = message as RtmImageMessage;
                                var messageBean = new MessageBean(_userId, rtmImageMessage, true)
                                {
                                    CacheFile = path
                                };
                                _messageBeanList.Add(messageBean);
                                _messageAdapter.NotifyItemRangeChanged(_messageBeanList.Count, 1);
                                _recyclerView.ScrollToPosition(_messageBeanList.Count - 1);

                                if(_isPeerToPeerMode)
                                {
                                    SendPeerMessage(rtmImageMessage);
                                }
                                else
                                {
                                    SendChannelMessage(rtmImageMessage);
                                }
                            });
                        };
                        resultCallback.OnFailureAction += (err) =>
                        {
                            RunOnUiThread(() => ShowToast($"Uploaded image with error {err.ErrorDescription}"));
                        };
                        ImageUtil.UploadImage(this, rtmClient, path, resultCallback);
                    }
                    break;
            }
        }

        public string GetPath(Android.Net.Uri uri)
        {
            string path = null;
            string[] projection = { MediaStore.MediaColumns.Data };
            var cr = ApplicationContext.ContentResolver;
            var metaCursor = cr.Query(uri, projection, null, null, null);
            if (metaCursor != null)
            {
                try
                {
                    if (metaCursor.MoveToFirst())
                    {
                        path = metaCursor.GetString(0);
                    }
                }
                finally
                {
                    metaCursor.Close();
                }

            }
            return path;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            switch(requestCode)
            {
                case ReadExternalStoragePermissionRequestCode:
                    if ((grantResults.Length == 1) && (grantResults[0] == Permission.Granted))
                    {
                        GetImageFromGalery();
                    }
                    else
                    {
                        Toast.MakeText(this, "Permission is missing", ToastLength.Short).Show();
                    }
                    break;
            }
        }

        public void OnClickFinish(object sender, System.EventArgs e)
        { 
            Finish();
        }

        /**
         * API CALL: send message to peer
         */
        private void SendPeerMessage(RtmMessage message)
        {
            rtmClient.SendMessageToPeer(_peerId, message, _chatManager.SendMessageOptions, _sendMessageClientCallback);
        }

        /**
         * API CALL: create and join channel
        */
        private void CreateAndJoinChannel() {

            // step 1: create a channel instance
            _rtmChannel = rtmClient.CreateChannel(_channelName, _channelListener);
            if (_rtmChannel == null) {
                ShowToast(GetString(Resource.String.join_channel_failed));
                Finish();
                return;
            }

            // step 2: join the channel
            _rtmChannel.Join(_channelJoinCallBack);
        }

        /**
         * API CALL: get channel member list
         */
        private void GetChannelMemberList() {
            _rtmChannel.GetMembers(_memberCallBack);
        }

        /**
         * API CALL: send message to a channel
         */
        private void SendChannelMessage(RtmMessage message)
        {
            _rtmChannel.SendMessage(message, _sendMessageChannelCallback);
        }

        /**
         * API CALL: leave and release channel
         */
        private void LeaveAndReleaseChannel() {
            if (_rtmChannel != null) {
                _rtmChannel.Leave(null);
                _rtmChannel.Release();
                _rtmChannel = null;
            }
        }

        private int GetMessageColor(string account) {
            for (int i = 0; i < _messageBeanList.Count; i++) {
                if (account.Equals(_messageBeanList[i].Account)) {
                    return _messageBeanList[i].Background;
                }
            }
            return MessageUtil.ColorList[MessageUtil.RandomGenerator.NextInt(MessageUtil.ColorList.Count)];
        }

        private void RefreshChannelTitle() {
            RunOnUiThread(() =>
            {
                string titleFormat = GetString(Resource.String.channel_title);
                string title = string.Format(titleFormat, _channelName, _channelMemberCount);
                _titleTextView.Text = title;
            });
        }
    }
}