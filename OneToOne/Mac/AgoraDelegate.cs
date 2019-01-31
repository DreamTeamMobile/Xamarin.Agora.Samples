using System;
using CoreGraphics;
using Foundation;
using Xamarin.Agora.Mac;

namespace DT.Samples.Agora.OneToOne.Mac
{
    public class AgoraDelegate : AgoraRtcEngineDelegate
    {
        private VideoChatViewController _controller;

        public AgoraDelegate(VideoChatViewController controller) : base()
        {
            _controller = controller;
        }

        public override void FirstRemoteVideoDecodedOfUid(AgoraRtcEngineKit engine, nuint uid, CGSize size, nint elapsed)
        {
            _controller.FirstRemoteVideoDecodedOfUid(engine, uid, size, elapsed);
        }

        public override void DidOfflineOfUid(AgoraRtcEngineKit engine, nuint uid, AgoraUserOfflineReason reason)
        {
            _controller.DidOfflineOfUid(engine, uid, reason);
        }

        public override void DidVideoMuted(AgoraRtcEngineKit engine, bool muted, nuint uid)
        {
            _controller.DidVideoMuted(engine, muted, uid);
        }
    }
}
