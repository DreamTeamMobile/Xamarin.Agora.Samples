using System;
using DT.Xamarin.Agora;

namespace DT.Samples.Agora.Voice.Droid
{
    public class RtcEngineEventHandler : IRtcEngineEventHandler
    {
        public Action DidJoinChannel;
        public Action<int> OnRemoteUserJoined;
        public Action<int> OnRemoteUserLeft;
        public Action<int, bool> OnRemoteUserVoiceMuted;

        public override void OnJoinChannelSuccess(string channel, int uid, int elapsed)
        {
            DidJoinChannel?.Invoke();
        }

        public override void OnUserJoined(int uid, int elapsed)
        {
            OnRemoteUserJoined?.Invoke(uid);
        }

        public override void OnUserOffline(int uid, int reason)
        {
            OnRemoteUserLeft?.Invoke(uid);
        }

        public override void OnUserMuteAudio(int uid, bool muted)
        {
            OnRemoteUserVoiceMuted?.Invoke(uid, muted);
        }
    }
}
