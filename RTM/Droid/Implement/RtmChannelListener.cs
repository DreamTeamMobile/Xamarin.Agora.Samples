using System;
using System.Collections.Generic;
using DT.Xamarin.Agora.Rtm;

namespace DT.Samples.Agora.Rtm.Droid
{
    public class RtmChannelListener : Java.Lang.Object, IRtmChannelListener
    {
        public Action<RtmMessage, RtmChannelMember> OnMessageReceivedAction;
        public Action<RtmChannelMember> OnMemberJoinedAction;
        public Action<RtmChannelMember> OnMemberLeftAction;

        public void OnAttributesDeleted(IDictionary<string, string> param)
        {
            // not supported yet
        }

        public void OnAttributesUpdated(IDictionary<string, string> param)
        {
            // not supported yet
        }

        public void OnAttributesUpdated(IList<RtmChannelAttribute> p0)
        {
            // not supported yet
        }

        public void OnFileMessageReceived(RtmFileMessage p0, RtmChannelMember p1)
        {
            // not supported yet
        }

        public void OnImageMessageReceived(RtmImageMessage message, RtmChannelMember member)
        {
            OnMessageReceivedAction?.Invoke(message, member);
        }

        public void OnMemberCountUpdated(int p0)
        {
            // not supported yet
        }

        public void OnMemberJoined(RtmChannelMember member)
        {
            OnMemberJoinedAction?.Invoke(member);
        }

        public void OnMemberLeft(RtmChannelMember member)
        {
            OnMemberLeftAction?.Invoke(member);
        }

        public void OnMessageReceived(RtmMessage message, RtmChannelMember member)
        {
            OnMessageReceivedAction?.Invoke(message, member);
        }
    }
}