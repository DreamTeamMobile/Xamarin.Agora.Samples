using System;
using DT.Xamarin.Agora;

namespace DT.Samples.Agora.Conference.iOS
{
    public class RtmChannelDelegate : AgoraRtmChannelDelegate
    {
        public Action<string, AgoraRtmMessage> OnMessageReceived;
        public Action<string, string> ShowAlert;

        public override void MemberJoined(AgoraRtmChannel channel, AgoraRtmMember member)
        {
            InvokeOnMainThread(() => ShowAlert(member.UserId, "Join"));
        }

        public override void MemberLeft(AgoraRtmChannel channel, AgoraRtmMember member)
        {
            InvokeOnMainThread(() => ShowAlert(member.UserId, "Left"));
        }

        public override void MessageReceived(AgoraRtmChannel channel, AgoraRtmMessage message, AgoraRtmMember member)
        {
            InvokeOnMainThread(() => OnMessageReceived(member.UserId, message));
        }
    }
}
