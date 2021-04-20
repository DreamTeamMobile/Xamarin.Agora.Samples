using System.Collections.Generic;
using DT.Samples.Agora.Shared;
using DT.Xamarin.Agora;

namespace DT.Samples.Agora.Rtm.Mac
{
    public enum LoginStatus
    {
        Online = 0,
        Offline = 1
    }

    public enum OneToOneMessageType
    {
        Normal = 0,
        Offline = 1
    }

    public class AgoraRtm
    {
        public static AgoraRtmKit RtmKit = new AgoraRtmKit(AgoraTestConstants.AgoraAPI, null);
        public static string Current;
        public static LoginStatus Status = LoginStatus.Offline;
        public static OneToOneMessageType OneToOneMessageType { get; set; } = OneToOneMessageType.Normal;

        private static Dictionary<string, List<AgoraRtmMessage>> _offlineMessages = new Dictionary<string, List<AgoraRtmMessage>>();

        public static void UpdateKit(AgoraRtmDelegate del)
        {
            if (RtmKit != null)
            {
                RtmKit.AgoraRtmDelegate = del;
            }
        }

        public static void AddOfflineMessage(AgoraRtmMessage offlineMessage, string user)
        {
            if (offlineMessage == null || !offlineMessage.IsOfflineMessage)
                return;

            if (!_offlineMessages.TryGetValue(user, out List<AgoraRtmMessage> userMessages))
            {
                userMessages = new List<AgoraRtmMessage>();
            }
            userMessages.Add(offlineMessage);
            _offlineMessages[user] = userMessages;
        }

        public static List<AgoraRtmMessage> GetOfflineMessages(string user)
        {
            return _offlineMessages.ContainsKey(user) ? _offlineMessages[user] : new List<AgoraRtmMessage>();
        }

        public static void RemoveAllOfflineMessages(string user)
        {
            if (_offlineMessages.ContainsKey(user))
                _offlineMessages[user].Clear();
        }
    }
}
