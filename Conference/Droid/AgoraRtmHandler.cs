using System;
using DT.Samples.Agora.Shared;
using DT.Xamarin.Agora.Rtm;
using Newtonsoft.Json;

namespace DT.Samples.Agora.Conference.Droid
{
    public class AgoraRtmHandler : Java.Lang.Object, IRtmClientListener
    {
        private RoomActivity _context;

        public AgoraRtmHandler(RoomActivity activity)
        {
            _context = activity;
        }

        public void OnConnectionStateChanged(int p0, int p1)
        {
            // not supported yet
        }

        public void OnMessageReceived(RtmMessage message, string peerId)
        {
            var signalData = JsonConvert.DeserializeObject<SignalMessage>(message.Text);
            _context.OnSignalReceived(signalData);
        }

        public void OnTokenExpired()
        {
            // not supported yet
        }
    }
}
