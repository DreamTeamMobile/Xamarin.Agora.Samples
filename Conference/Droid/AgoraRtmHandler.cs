using System;
using System.Collections.Generic;
using DT.Samples.Agora.Shared;
using DT.Xamarin.Agora.Rtm;
using Java.Lang;
using Newtonsoft.Json;

namespace DT.Samples.Agora.Conference.Droid
{
    public class AgoraRtmHandler : Java.Lang.Object, IRtmClientListener
    {
        public Action<SignalMessage> OnSignalReceived;

        public AgoraRtmHandler()
        {
        }

        public void OnConnectionStateChanged(int p0, int p1)
        {
            // not supported yet
        }

        public void OnFileMessageReceivedFromPeer(RtmFileMessage p0, string p1)
        {
            // not supported yet
        }

        public void OnImageMessageReceivedFromPeer(RtmImageMessage p0, string p1)
        {
            // not supported yet
        }

        public void OnMediaDownloadingProgress(RtmMediaOperationProgress p0, long p1)
        {
            // not supported yet
        }

        public void OnMediaUploadingProgress(RtmMediaOperationProgress p0, long p1)
        {
            // not supported yet
        }

        public void OnMessageReceived(RtmMessage message, string peerId)
        {
            var signalData = JsonConvert.DeserializeObject<SignalMessage>(message.Text);
            OnSignalReceived?.Invoke(signalData);
        }

        public void OnPeersOnlineStatusChanged(IDictionary<string, Integer> p0)
        {
            // not supported yet
        }

        public void OnTokenExpired()
        {
            // not supported yet
        }
    }
}
