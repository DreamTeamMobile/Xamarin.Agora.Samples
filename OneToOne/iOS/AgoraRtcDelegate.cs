using System;
using DT.Xamarin.Agora;

namespace DT.Samples.Agora.OneToOne.iOS
{
	public class AgoraRtcDelegate : AgoraRtcEngineDelegate
	{
		private RoomViewController _controller;

		public AgoraRtcDelegate(RoomViewController controller) : base()
		{
			_controller = controller;
		}

		public override void FirstRemoteVideoDecodedOfUid(AgoraRtcEngineKit engine, nuint uid, CoreGraphics.CGSize size, nint elapsed)
		{
			_controller.FirstRemoteVideoDecodedOfUid(engine, uid, size, elapsed);
		}

		public override void DidOfflineOfUid(AgoraRtcEngineKit engine, nuint uid, AgoraRtcUserOfflineReason reason)
		{
			_controller.DidOfflineOfUid(engine, uid, reason);
		}

		public override void DidVideoMuted(AgoraRtcEngineKit engine, bool muted, nuint uid)
		{
			_controller.DidVideoMuted(engine, muted, uid);
		}

		public override void FirstLocalVideoFrameWithSize(AgoraRtcEngineKit engine, CoreGraphics.CGSize size, nint elapsed)
		{
			_controller.FirstLocalVideoFrameWithSize(engine, size, elapsed);
		}
	}
}
