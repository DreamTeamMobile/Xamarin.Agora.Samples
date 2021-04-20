using System;
using System.Collections.Generic;
using DT.Xamarin.Agora.Rtm;
using Java.Lang;

namespace DT.Samples.Agora.Rtm.Droid
{
    public class RtmClientListener : Java.Lang.Object, IRtmClientListener
    {
        public Action<int, int> StateChanged;
        public Action<RtmMessage, string> MessageReceived;

        public void OnConnectionStateChanged(int state, int reason)
        {
            StateChanged?.Invoke(state, reason);
        }

        public void OnFileMessageReceivedFromPeer(RtmFileMessage p0, string p1)
        {
            // not supported yet
        }

        public void OnImageMessageReceivedFromPeer(RtmImageMessage message, string peerId)
        {
            MessageReceived?.Invoke(message, peerId);
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
            MessageReceived?.Invoke(message, peerId);
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