using System;
using DT.Xamarin.Agora;

namespace DT.Samples.Agora.Cross.iOS
{
    /// <summary>
    /// Agora rtc delegate.
    /// </summary>
    public class AgoraRtcDelegate : AgoraRtcEngineDelegate
    {
        private AgoraServiceImplementation _controller;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Xamarin.Agora.Full.Forms.AgoraRtcDelegate"/> class.
        /// </summary>
        /// <param name="controller">Controller.</param>
        public AgoraRtcDelegate(AgoraServiceImplementation controller) : base()
        {
            _controller = controller;
        }
        /// <summary>
        /// Firsts the remote video decoded of uid.
        /// </summary>
        /// <param name="engine">Engine.</param>
        /// <param name="uid">Uid.</param>
        /// <param name="size">Size.</param>
        /// <param name="elapsed">Elapsed.</param>
        public override void FirstRemoteVideoDecodedOfUid(AgoraRtcEngineKit engine, nuint uid, CoreGraphics.CGSize size, nint elapsed)
        {
            Console.WriteLine($"FirstRemoteVideoDecodedOfUid: {uid}, {size}, {elapsed}");
            _controller.FirstRemoteVideoDecodedOfUid(engine, uid, size, elapsed);
        }
        /// <summary>
        /// Dids the offline of uid.
        /// </summary>
        /// <param name="engine">Engine.</param>
        /// <param name="uid">Uid.</param>
        /// <param name="reason">Reason.</param>
        public override void DidOfflineOfUid(AgoraRtcEngineKit engine, nuint uid, UserOfflineReason reason)
        {
            Console.WriteLine($"DidOfflineOfUid: {uid}, {reason}");
            _controller.OnUserOffline(engine, uid, reason);
        }
        /// <summary>
        /// Dids the video muted.
        /// </summary>
        /// <param name="engine">Engine.</param>
        /// <param name="muted">If set to <c>true</c> muted.</param>
        /// <param name="uid">Uid.</param>
        public override void DidVideoMuted(AgoraRtcEngineKit engine, bool muted, nuint uid)
        {
            Console.WriteLine($"DidVideoMuted: {muted}, {uid}");
            _controller.OnUserMuteVideo(engine, uid, muted);
        }
        /// <summary>
        /// Audios the quality of uid.
        /// </summary>
        /// <param name="engine">Engine.</param>
        /// <param name="uid">Uid.</param>
        /// <param name="quality">Quality.</param>
        /// <param name="delay">Delay.</param>
        /// <param name="lost">Lost.</param>
        public override void AudioQualityOfUid(AgoraRtcEngineKit engine, nuint uid, Quality quality, nuint delay, nuint lost)
        {
            Console.WriteLine($"AudioQualityOfUid: {uid}, {quality.ToString()}, {delay}, {lost}");
        }
        /// <summary>
        /// Dids the join channel.
        /// </summary>
        /// <param name="engine">Engine.</param>
        /// <param name="channel">Channel.</param>
        /// <param name="uid">Uid.</param>
        /// <param name="elapsed">Elapsed.</param>
        public override void DidJoinChannel(AgoraRtcEngineKit engine, string channel, nuint uid, nint elapsed)
        {
            Console.WriteLine($"DidJoinChannel: {engine}, {channel}, {uid}, {elapsed}");
        }
        /// <summary>
        /// Dids the audio muted.
        /// </summary>
        /// <param name="engine">Engine.</param>
        /// <param name="muted">If set to <c>true</c> muted.</param>
        /// <param name="uid">Uid.</param>
        public override void DidAudioMuted(AgoraRtcEngineKit engine, bool muted, nuint uid)
        {
            Console.WriteLine($"DidAudioMuted: {engine}, {muted}, {uid}");
            _controller.OnUserMuteAudio(engine, uid, muted);
        }
        /// <summary>
        /// Dids the joined of uid.
        /// </summary>
        /// <param name="engine">Engine.</param>
        /// <param name="uid">Uid.</param>
        /// <param name="elapsed">Elapsed.</param>
        public override void DidJoinedOfUid(AgoraRtcEngineKit engine, nuint uid, nint elapsed)
        {
            Console.WriteLine($"DidJoinedOfUid: {engine}, {uid}, {elapsed}");
            _controller.OnUserJoined(engine, uid);
        }
        /// <summary>
        /// Dids the occur error.
        /// </summary>
        /// <param name="engine">Engine.</param>
        /// <param name="errorCode">Error code.</param>
        public override void DidOccurError(AgoraRtcEngineKit engine, ErrorCode errorCode)
        {
            Console.WriteLine($"DidOccurError: {engine}, {errorCode}");
        }
        /// <summary>
        /// Dids the occur stream message error from uid.
        /// </summary>
        /// <param name="engine">Engine.</param>
        /// <param name="uid">Uid.</param>
        /// <param name="streamId">Stream identifier.</param>
        /// <param name="error">Error.</param>
        /// <param name="missed">Missed.</param>
        /// <param name="cached">Cached.</param>
        public override void DidOccurStreamMessageErrorFromUid(AgoraRtcEngineKit engine, nuint uid, nint streamId, nint error, nint missed, nint cached)
        {
            Console.WriteLine($"DidOccurStreamMessageErrorFromUid: {uid}, {streamId}, {error}, {missed}, {cached}");
        }
        /// <summary>
        /// Dids the occur warning.
        /// </summary>
        /// <param name="engine">Engine.</param>
        /// <param name="warningCode">Warning code.</param>
        public override void DidOccurWarning(AgoraRtcEngineKit engine, WarningCode warningCode)
        {
            Console.WriteLine($"DidOccurWarning:  {warningCode}");
        }
        /// <summary>
        /// Dids the video enabled.
        /// </summary>
        /// <param name="engine">Engine.</param>
        /// <param name="enabled">If set to <c>true</c> enabled.</param>
        /// <param name="uid">Uid.</param>
        public override void DidVideoEnabled(AgoraRtcEngineKit engine, bool enabled, nuint uid)
        {
            Console.WriteLine($"DidVideoEnabled: {enabled}, {uid}");
            _controller.OnUserEnabledVideo(engine, uid, enabled);
        }
        /// <summary>
        /// Dids the rejoin channel.
        /// </summary>
        /// <param name="engine">Engine.</param>
        /// <param name="channel">Channel.</param>
        /// <param name="uid">Uid.</param>
        /// <param name="elapsed">Elapsed.</param>
        public override void DidRejoinChannel(AgoraRtcEngineKit engine, string channel, nuint uid, nint elapsed)
        {
            Console.WriteLine($"DidRejoinChannel: {channel}, {uid}, {elapsed}");
        }
        /// <summary>
        /// Firsts the remote video frame of uid.
        /// </summary>
        /// <param name="engine">Engine.</param>
        /// <param name="uid">Uid.</param>
        /// <param name="size">Size.</param>
        /// <param name="elapsed">Elapsed.</param>
        public override void FirstRemoteVideoFrameOfUid(AgoraRtcEngineKit engine, nuint uid, CoreGraphics.CGSize size, nint elapsed)
        {
            Console.WriteLine($"FirstRemoteVideoFrameOfUid: {uid}, {size}, {elapsed}");
        }
        /// <summary>
        /// Firsts the size of the local video frame with.
        /// </summary>
        /// <param name="engine">Engine.</param>
        /// <param name="size">Size.</param>
        /// <param name="elapsed">Elapsed.</param>
        public override void FirstLocalVideoFrameWithSize(AgoraRtcEngineKit engine, CoreGraphics.CGSize size, nint elapsed)
        {
            Console.WriteLine($"FirstLocalVideoFrameWithSize: {size}, {elapsed}");
            //_controller.FirstLocalVideoFrameWithSize(engine, size, elapsed);
        }

        /// <summary>
        /// Lastmiles the quality.
        /// </summary>
        /// <param name="engine">Engine.</param>
        /// <param name="quality">Quality.</param>
        public override void LastmileQuality(AgoraRtcEngineKit engine, Quality quality)
        {
            //Console.WriteLine($"LastmileQuality: {quality.ToString()}");
        }
    }
}
