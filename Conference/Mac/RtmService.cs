using System;
using System.Threading.Tasks;
using DT.Samples.Agora.Shared;
using DT.Xamarin.Agora;
using Newtonsoft.Json;

namespace DT.Samples.Agora.Conference.iOS
{
    public class RtmService
    {
        private AgoraRtmChannel _rtmChannel;
        private AgoraRtmKit _rtmKit = new AgoraRtmKit(AgoraTestConstants.AgoraAPI, null);

        public static RtmService Instance { get; } = new RtmService();
        public string UserName { get; private set; }

        public event Action<bool> OnLogin;
        public event Action<bool> OnJoinChannel;
        public event Action<bool> OnSendMessage;
        public event Action<SignalMessage> OnSignalReceived;

        public RtmService()
        {
        }

        public async Task Init(string userId)
        {
            var rtmToken = await AgoraTokenService.GetRtmToken(userId);
            _rtmKit.LoginByToken(rtmToken, userId, (status) =>
            {
                var success = status == AgoraRtmLoginErrorCode.Ok;
                OnLogin?.Invoke(success);
                if (success)
                {
                    UserName = userId;
                    var rtmDelegate = new RtmDelegate();
                    rtmDelegate.OnMessageReceived += OmMessageReceived;
                    _rtmKit.AgoraRtmDelegate = rtmDelegate;
                }
            });
        }

        public void JoinChannel(string channel)
        {
            var channelDelegate = new RtmChannelDelegate();
            channelDelegate.OnMessageReceived += OmMessageReceived;
            channelDelegate.ShowAlert += (user, msg) => Console.WriteLine($"RTM alert. {user}: {msg}"); ;

            _rtmChannel = _rtmKit.CreateChannelWithId(channel, channelDelegate);

            if (_rtmChannel == null)
                return;

            _rtmChannel.JoinWithCompletion(JoinChannelBlock);
        }

        public void LeaveChannel()
        {
            if (_rtmChannel != null)
            {
                _rtmChannel.LeaveWithCompletion(LeaveChannelBlock);
                _rtmChannel = null;
            }
        }

        public void SendChannelMessage(string text)
        {
            var rtmMessage = new AgoraRtmMessage(text);
            _rtmChannel.SendMessage(rtmMessage, (state) =>
            {
                Console.WriteLine($"RTM send channel msg state: {state}");
                OnSendMessage?.Invoke(state == AgoraRtmSendChannelMessageErrorCode.Ok);
            });
        }

        public void SendPeerMessage(string peerId, string text)
        {
            var rtmMessage = new AgoraRtmMessage(text);
            _rtmKit.SendMessage(rtmMessage, peerId, (state) =>
            {
                Console.WriteLine($"RTM send peer msg state: {state}");
                OnSendMessage?.Invoke(state == AgoraRtmSendPeerMessageErrorCode.Ok);
            });
        }

        public void Logout()
        {
            _rtmKit.LogoutWithCompletion(LogoutBlock);
        }

        private void OmMessageReceived(string peerId, AgoraRtmMessage message)
        {
            var text = message.Text;
            var signal = JsonConvert.DeserializeObject<SignalMessage>(text);
            OnSignalReceived?.Invoke(signal);
        }

        private void JoinChannelBlock(AgoraRtmJoinChannelErrorCode errorCode)
        {
            var success = errorCode == AgoraRtmJoinChannelErrorCode.Ok;
            OnJoinChannel?.Invoke(success);
            if (success)
            {
                Console.WriteLine($"RTM join channel successsful");
            }
            else
            {
                Console.WriteLine($"RTM join channel error: {errorCode}");
            }
        }

        private void LeaveChannelBlock(AgoraRtmLeaveChannelErrorCode errorCode)
        {
            var success = errorCode == AgoraRtmLeaveChannelErrorCode.Ok;
            if (success)
            {
                Console.WriteLine($"RTM leave channel successsful");
            }
            else
            {
                Console.WriteLine($"RTM leave channel error: {errorCode}");
            }
        }

        private void LogoutBlock(AgoraRtmLogoutErrorCode errorCode)
        {
            var success = errorCode == AgoraRtmLogoutErrorCode.Ok;
            if (success)
            {
                Console.WriteLine($"RTM logout channel successsful");
            }
            else
            {
                Console.WriteLine($"RTM logout channel error: {errorCode}");
            }
        }
    }
}
