using System;
using System.Diagnostics;
using DT.Xamarin.Agora;

namespace DT.Samples.Agora.Conference.iOS
{
    public class AgoraRtcDelegate : AgoraRtcEngineDelegate
    {
        private RoomViewController _controller;

        public AgoraRtcDelegate(RoomViewController controller) : base()
        {
            _controller = controller;
        }

        public override void DidJoinedOfUid(AgoraRtcEngineKit engine, nuint uid, nint elapsed)
        {
            Debug.WriteLine($"DidJoinedOfUid {uid}");
        }

        public override void FirstRemoteVideoDecodedOfUid(AgoraRtcEngineKit engine, nuint uid, CoreGraphics.CGSize size, nint elapsed)
        {
            Debug.WriteLine($"FirstRemoteVideoDecodedOfUid {uid}");
            _controller.FirstRemoteVideoDecodedOfUid(engine, uid, size, elapsed);
        }

        public override void DidOfflineOfUid(AgoraRtcEngineKit engine, nuint uid, UserOfflineReason reason)
        {
            Debug.WriteLine($"DidOfflineOfUid {uid}");
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

        public override void FirstLocalVideoFrameWithSize(AgoraRtcEngineKit engine, CoreGraphics.CGSize size, nint elapsed)
        {
            _controller.FirstLocalVideoFrameWithSize(engine, size, elapsed);
        }

        public override void TokenPrivilegeWillExpire(AgoraRtcEngineKit engine, string token)
        {
            _controller.TokenPrivilegeWillExpire(engine, token);
        }
    }
}
