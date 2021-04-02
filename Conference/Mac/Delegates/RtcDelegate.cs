using System;
using CoreGraphics;
using Foundation;
using Xamarin.Agora.Mac;

namespace DT.Samples.Agora.Conference.iOS
{
    public class RtcDelegate : AgoraRtcEngineDelegate
    {
        private VideoChatViewController _controller;

        public RtcDelegate(VideoChatViewController controller) : base()
        {
            _controller = controller;
        }

        public override void FirstRemoteVideoDecodedOfUid(AgoraRtcEngineKit engine, nuint uid, CGSize size, nint elapsed)
        {
            _controller.FirstRemoteVideoDecodedOfUid(engine, uid, size, elapsed);
        }

        public override void DidOfflineOfUid(AgoraRtcEngineKit engine, nuint uid, UserOfflineReason reason)
        {
            _controller.DidOfflineOfUid(engine, uid, reason);
        }

        public override void DidVideoMuted(AgoraRtcEngineKit engine, bool muted, nuint uid)
        {
            _controller.DidVideoMuted(engine, muted, uid);
        }

        public override void DidAudioMuted(AgoraRtcEngineKit engine, bool muted, nuint uid)
        {
            _controller.DidAudioMuted(engine, muted, uid);
        }

        public override void TokenPrivilegeWillExpire(AgoraRtcEngineKit engine, string token)
        {
            _controller.TokenPrivilegeWillExpire(engine, token);
        }
    }
}
