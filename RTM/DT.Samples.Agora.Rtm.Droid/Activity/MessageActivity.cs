using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Widget;
using DT.Xamarin.Agora.Rtm;
using Android.Runtime;

namespace DT.Samples.Agora.Rtm.Droid
{
    [Activity(Label = "MessageActivity")]
    public class MessageActivity : AppCompatActivity 
    {

        private TextView _titleTextView;
        private TextView _btnSend;
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

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityMessage);

            InitUIAndData();
            InitChat();
        }

        private void PeerJoin() 
        {
            var joinText = $"{_userId} join";

            var messageBean = new MessageBean(_userId, joinText, true)
            {
                Background = GetMessageColor(_userId)
            };

            RunOnUiThread(() =>
            {
                _messageBeanList.Add(messageBean);
                _messageAdapter.NotifyItemRangeChanged(_messageBeanList.Count, 1);
                _recyclerView.ScrollToPosition(_messageBeanList.Count - 1);
            });
        }

        private void OnMessageReceived(RtmMessage message, RtmChannelMember fromMember)
        {
            string account = fromMember.UserId;
            string msg = message.Text;
            MessageBean messageBean = new MessageBean(account, msg, false)
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
                string content = message.Text;
                if (_peerId.Equals(peerId))
                {
                    MessageBean messageBean = new MessageBean(peerId, content, false)
                    {
                        Background = GetMessageColor(peerId)
                    };

                    _messageBeanList.Add(messageBean);
                    _messageAdapter.NotifyItemRangeChanged(_messageBeanList.Count, 1);
                    _recyclerView.ScrollToPosition(_messageBeanList.Count - 1);
                }
                else
                {
                    MessageUtil.AddMessageBean(peerId, content);
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
            _titleTextView = FindViewById<TextView>(Resource.Id.message_title);
            _msgEditText = FindViewById<EditText>(Resource.Id.message_edittiext);
            _recyclerView = FindViewById<RecyclerView>(Resource.Id.message_list);
            _btnSend = FindViewById<TextView>(Resource.Id.selection_chat_btn);
            _btnBack = FindViewById<ImageView>(Resource.Id.back);

            _btnBack.Click += OnClickFinish;
            _btnSend.Click += OnClickSend;

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
            InitCallbackAndListener();

            _chatManager = MainApplication.ChatManager;
            rtmClient = _chatManager.GetRtmClient();

            _chatManager.RegisterListener(_myRtmClientListener);

            if (!_isPeerToPeerMode)
            {
                CreateAndJoinChannel();
            }

            PeerJoin();
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
            }
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

        public void OnClickSend(object sender, EventArgs e)
        {
            string msg = _msgEditText.Text;
            if (!msg.Equals("")) {
                MessageBean messageBean = new MessageBean(_userId, msg, true);
                _messageBeanList.Add(messageBean);
                _messageAdapter.NotifyItemRangeChanged(_messageBeanList.Count, 1);
                _recyclerView.ScrollToPosition(_messageBeanList.Count - 1);
                if (_isPeerToPeerMode) {
                    SendPeerMessage(msg);
                } else {
                    SendChannelMessage(msg);
                }
            }
            _msgEditText.Text = "";
        }

        public void OnClickFinish(object sender, System.EventArgs e)
        { 
            Finish();
        }

        /**
         * API CALL: send message to peer
         */
        private void SendPeerMessage(string content) {

            // step 1: create a message
            var message = rtmClient.CreateMessage();
            message.Text = content;

            // step 2: send message to peer
            rtmClient.SendMessageToPeer(_peerId, message, _sendMessageClientCallback);
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
        private void SendChannelMessage(string content)
        {
            // step 1: create a message
            RtmMessage message = rtmClient.CreateMessage();
            message.Text = content;

            // step 2: send message to channel
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