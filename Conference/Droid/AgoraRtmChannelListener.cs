using System;
using System.Collections.Generic;
using DT.Samples.Agora.Shared;
using DT.Xamarin.Agora.Rtm;
using Newtonsoft.Json;

namespace DT.Samples.Agora.Conference.Droid
{
    public class AgoraRtmChannelListener : Java.Lang.Object, IRtmChannelListener
    {
        private RoomActivity _context;

        public AgoraRtmChannelListener(RoomActivity activity)
        {
            _context = activity;
        }

        public void OnAttributesUpdated(IList<RtmChannelAttribute> p0)
        {
            // not supported yet
        }

        public void OnMemberCountUpdated(int p0)
        {
            // not supported yet
        }

        public void OnMemberJoined(RtmChannelMember p0)
        {
            // not supported yet
        }

        public void OnMemberLeft(RtmChannelMember p0)
        {
            // not supported yet
        }

        public void OnMessageReceived(RtmMessage message, RtmChannelMember p1)
        {
            var signalData = JsonConvert.DeserializeObject<SignalMessage>(message.Text);
            _context.OnSignalReceived(signalData);
        }
    }
}
