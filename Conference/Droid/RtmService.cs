using System;
using System.Threading.Tasks;
using Android.Content;
using DT.Samples.Agora.Shared;
using DT.Xamarin.Agora.Rtm;

namespace DT.Samples.Agora.Conference.Droid
{
    public class RtmService
    {
        private Context _context;
        private RtmClient _rtmClient;
        private RtmChannel _rtmChannel;
        private AgoraRtmHandler _agoraRtmHandler;
        private ResultCallback _sendMessageChannelCallback;
        private AgoraRtmChannelListener _channelListener;

        public event Action<bool> OnLogin;
        public event Action<bool> OnJoinChannel;
        public event Action<bool> OnSendMessage;
        public event Action<SignalMessage> OnSignalReceived;

        public static RtmService Instance { get; } = new RtmService();
        public string UserName { get; private set; }

        public RtmService()
        {
        }

        public async Task Init(Context context, string userId)
        {
            _context = context;
            _agoraRtmHandler = new AgoraRtmHandler();
            _agoraRtmHandler.OnSignalReceived += (signal) =>
            {
                Log($"Signal received from {signal.RtcPeerId}");
                OnSignalReceived?.Invoke(signal);
            };
            _rtmClient = RtmClient.CreateInstance(_context, AgoraTestConstants.AgoraAPI, _agoraRtmHandler);

            var rtmToken = await AgoraTokenService.GetRtmToken(userId);
            var loginCallback = new ResultCallback();
            loginCallback.OnSuccessAction += (obj) =>
            {
                UserName = userId;
                OnLogin?.Invoke(true);
                Log("Login Success");
            };
            loginCallback.OnFailureAction += (err) =>
            {
                if (_rtmChannel == null)
                    return;
                OnLogin?.Invoke(false);
                Log("Login Fail");
            };
            _rtmClient.Login(rtmToken, userId, loginCallback);
        }

        public void JoinChannel(string channel)
        {
            Log($"Joining to channel [{channel}]");
            _channelListener = new AgoraRtmChannelListener();
            _channelListener.OnSignalReceived += (signal) =>
            {
                Log($"Signal received from {signal.RtcPeerId}");
                OnSignalReceived?.Invoke(signal);
            };
            _sendMessageChannelCallback = new ResultCallback();
            _sendMessageChannelCallback.OnSuccessAction += (obj) =>
            {
                Log("Send channel message Success");
                OnSendMessage?.Invoke(true);
            };
            _sendMessageChannelCallback.OnFailureAction += (obj) =>
            {
                Log("Send channel message Fail");
                OnSendMessage?.Invoke(false);
            };
            _rtmChannel = _rtmClient.CreateChannel(channel, _channelListener);
            var channelJoinCallBack = new ResultCallback();
            channelJoinCallBack.OnSuccessAction += (obj) =>
            {
                Log("Join Success");
                if (_rtmChannel == null)
                    return;
                OnJoinChannel?.Invoke(true);
            };
            channelJoinCallBack.OnFailureAction += (err) =>
            {
                Log($"Join Fail. {err.ErrorDescription}");
                if (_rtmChannel == null)
                    return;
                OnJoinChannel?.Invoke(false);
            };
            _rtmChannel?.Join(channelJoinCallBack);
        }

        public void LeaveChannel()
        {
            if (_rtmChannel != null)
            {
                var resultCallback = new ResultCallback();
                resultCallback.OnSuccessAction += (obj) =>
                {
                    Log("Logout success");
                };
                resultCallback.OnFailureAction += (err) =>
                {
                    Log($"Logout fail. {err.ErrorDescription}");
                };

                _rtmChannel.Leave(resultCallback);
                _rtmChannel.Release();
                _rtmChannel = null;
            }
        }

        public void SendChannelMessage(string text)
        {
            var message = _rtmClient.CreateMessage();
            message.Text = text;
            _rtmChannel.SendMessage(message, _sendMessageChannelCallback);
        }

        public void SendPeerMessage(string peerId, string text)
        {
            var message = _rtmClient.CreateMessage();
            message.Text = text;
            var options = new SendMessageOptions
            {
                EnableOfflineMessaging = true
            };
            _rtmClient.SendMessageToPeer(peerId, message, options, _sendMessageChannelCallback);
        }

        private void Log(string msg)
        {
            Android.Util.Log.Debug("RTMService", msg);
        }

        public void Logout()
        {
            _rtmClient.Logout(null);
        }
    }
}
