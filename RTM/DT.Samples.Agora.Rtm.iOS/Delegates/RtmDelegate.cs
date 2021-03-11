using System;
using DT.Xamarin.Agora;

namespace DT.Samples.Agora.Rtm.iOS
{
    public class RtmDelegate : AgoraRtmDelegate
    {
        public Action<string, AgoraRtmMessage> AppendMessage;

        public override void ConnectionStateChanged(AgoraRtmKit kit, AgoraRtmConnectionState state, AgoraRtmConnectionChangeReason reason)
        {
            Console.WriteLine($"connection state changed: {state}");
        }

        public override void MessageReceived(AgoraRtmKit kit, AgoraRtmMessage message, string peerId)
        {
            AppendMessage(peerId, message);
        }
    }
}