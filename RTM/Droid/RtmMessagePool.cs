using System;
using System.Collections.Generic;
using DT.Xamarin.Agora.Rtm;

namespace DT.Samples.Agora.Rtm.Droid
{
    public class RtmMessagePool
    {
        private Dictionary<string, List<RtmMessage>> _offlineMessageMap = new Dictionary<string, List<RtmMessage>>();

        public void InsertOfflineMessage(RtmMessage rtmMessage, string peerId)
        {
            var messageList = new List<RtmMessage>();
            if (_offlineMessageMap.ContainsKey(peerId))
            {
                messageList = _offlineMessageMap[peerId];
            }
            messageList.Add(rtmMessage);
            _offlineMessageMap[peerId] = messageList;
        }

        public List<RtmMessage> GetAllOfflineMessages(string peerId)
        {
            return _offlineMessageMap.ContainsKey(peerId) ? _offlineMessageMap[peerId] : new List<RtmMessage>();
        }

        public void RemoveAllOfflineMessages(string peerId)
        {
            _offlineMessageMap.Remove(peerId);
        }
    }
}
