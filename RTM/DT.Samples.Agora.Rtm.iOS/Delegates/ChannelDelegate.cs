using System;
using DT.Xamarin.Agora;

namespace DT.Samples.Agora.Rtm.iOS
{
    public class ChannelDelegate : AgoraRtmChannelDelegate
    {
        public Action<string, string> AppendMessage;
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
            InvokeOnMainThread(() => AppendMessage(member.UserId, message.Text));
        }
    }
}