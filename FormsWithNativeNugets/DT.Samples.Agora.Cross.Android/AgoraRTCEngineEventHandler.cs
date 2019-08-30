using System;
using Android.Graphics;
using DT.Xamarin.Agora;

namespace DT.Samples.Agora.Cross.Droid
{
    /// <summary>
    /// Agora rtc engine event handler.
    /// </summary>
    public class AgoraRtcEngineEventHandler : IRtcEngineEventHandler
    {
        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected override void Dispose(bool disposing)
        {
            _agoraService = null;
            base.Dispose(disposing);
        }

        private AgoraServiceImplementation _agoraService;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Xamarin.Agora.Full.Forms.AgoraRtcEngineEventHandler"/> class.
        /// </summary>
        /// <param name="agoraService">Agora service.</param>
        public AgoraRtcEngineEventHandler(AgoraServiceImplementation agoraService)
        {
            _agoraService = agoraService;
        }
        /// <summary>
        /// On the join channel success.
        /// </summary>
        /// <param name="p0">Channel.</param>
        /// <param name="p1">Uid.</param>
        /// <param name="p2">Elapsed.</param>
        public override void OnJoinChannelSuccess(string p0, int p1, int p2)
        {
            _agoraService.OnJoinChannelSuccess(p0, p1, p2);
        }
        /// <summary>
        /// Ons the first remote video decoded.
        /// </summary>
        /// <param name="p0">Uid.</param>
        /// <param name="p1">Width.</param>
        /// <param name="p2">Height.</param>
        /// <param name="p3">Elapsed.</param>
        public override void OnFirstRemoteVideoDecoded(int p0, int p1, int p2, int p3)
        {
            _agoraService.OnFirstRemoteVideoDecoded(p0, p1, p2, p3);
        }
        /// <summary>
        /// Ons the user offline.
        /// </summary>
        /// <param name="p0">Uid.</param>
        /// <param name="p1">Reason.</param>
        public override void OnUserOffline(int p0, int p1)
        {
            _agoraService.OnUserOffline(p0, p1);
        }
        /// <summary>
        /// Ons the user mute video.
        /// </summary>
        /// <param name="p0">P0.</param>
        /// <param name="p1">If set to <c>true</c> p1.</param>
        public override void OnUserMuteVideo(int p0, bool p1)
        {
            _agoraService.OnUserMuteVideo(p0, p1);
        }
        /// <summary>
        /// Ons the connection interrupted.
        /// </summary>
        public override void OnConnectionInterrupted()
        {
            _agoraService.OnConnectionInterrupted();
        }
        /// <summary>
        /// Ons the connection lost.
        /// </summary>
        public override void OnConnectionLost()
        {
            _agoraService.OnConnectionLost();
        }
        /// <summary>
        /// Ons the error.
        /// </summary>
        /// <param name="p0">P0.</param>
        public override void OnError(int p0)
        {
            _agoraService.OnError(p0);
        }
        /// <summary>
        /// Ons the lastmile quality.
        /// </summary>
        /// <param name="p0">P0.</param>
        public override void OnLastmileQuality(int p0)
        {
            _agoraService.OnLastmileQuality(p0);
        }
        /// <summary>
        /// Ons the rejoin channel success.
        /// </summary>
        /// <param name="p0">P0.</param>
        /// <param name="p1">P1.</param>
        /// <param name="p2">P2.</param>
        public override void OnRejoinChannelSuccess(string p0, int p1, int p2)
        {
            _agoraService.OnRejoinChannelSuccess(p0, p1, p2);
        }
        /// <summary>
        /// Ons the warning.
        /// </summary>
        /// <param name="p0">P0.</param>
        public override void OnWarning(int p0)
        {
            _agoraService.OnWarning(p0);
        }
        /// <summary>
        /// Ons the leave channel.
        /// </summary>
        /// <param name="p0">P0.</param>
        public override void OnLeaveChannel(IRtcEngineEventHandler.RtcStats p0)
        {
            _agoraService.OnLeaveChannel(p0);
        }
        /// <summary>
        /// Ons the rtc stats.
        /// </summary>
        /// <param name="p0">P0.</param>
        public override void OnRtcStats(IRtcEngineEventHandler.RtcStats p0)
        {
            _agoraService.OnRtcStats(p0);
        }
        /// <summary>
        /// Ons the video stopped.
        /// </summary>
        public override void OnVideoStopped()
        {
            _agoraService.OnVideoStopped();
        }
        /// <summary>
        /// Ons the active speaker.
        /// </summary>
        /// <param name="p0">P0.</param>
        public override void OnActiveSpeaker(int p0)
        {
            _agoraService.OnActiveSpeaker(p0);
        }
        /// <summary>
        /// Ons the API call executed.
        /// </summary>
        /// <param name="p0">P0.</param>
        /// <param name="p1">P1.</param>
        /// <param name="p2">P2.</param>
        public override void OnApiCallExecuted(int p0, string p1, string p2)
        {
            _agoraService.OnApiCallExecuted(p0, p1, p2);
        }
        /// <summary>
        /// Ons the audio effect finished.
        /// </summary>
        /// <param name="p0">P0.</param>
        public override void OnAudioEffectFinished(int p0)
        {
            _agoraService.OnAudioEffectFinished(p0);
        }
        /// <summary>
        /// Ons the audio mixing finished.
        /// </summary>
        public override void OnAudioMixingFinished()
        {
            _agoraService.OnAudioMixingFinished();
        }
        /// <summary>
        /// Ons the audio quality.
        /// </summary>
        /// <param name="p0">P0.</param>
        /// <param name="p1">P1.</param>
        /// <param name="p2">P2.</param>
        /// <param name="p3">P3.</param>
        public override void OnAudioQuality(int p0, int p1, short p2, short p3)
        {
            _agoraService.OnAudioQuality(p0, p1, p2, p3);
        }
        /// <summary>
        /// Ons the audio route changed.
        /// </summary>
        /// <param name="p0">P0.</param>
        public override void OnAudioRouteChanged(int p0)
        {
            _agoraService.OnAudioRouteChanged(p0);
        }
        /// <summary>
        /// Ons the audio volume indication.
        /// </summary>
        /// <param name="p0">P0.</param>
        /// <param name="p1">P1.</param>
        public override void OnAudioVolumeIndication(AudioVolumeInfo[] p0, int p1)
        {
            _agoraService.OnAudioVolumeIndication(p0, p1);
        }
        /// <summary>
        /// Ons the camera focus area changed.
        /// </summary>
        /// <param name="p0">P0.</param>
        public override void OnCameraFocusAreaChanged(Rect p0)
        {
            _agoraService.OnCameraFocusAreaChanged(p0);
        }
        /// <summary>
        /// Ons the camera ready.
        /// </summary>
        public override void OnCameraReady()
        {
            _agoraService.OnCameraReady();
        }
        /// <summary>
        /// Ons the client role changed.
        /// </summary>
        /// <param name="p0">P0.</param>
        /// <param name="p1">P1.</param>
        public override void OnClientRoleChanged(int p0, int p1)
        {
            _agoraService.OnClientRoleChanged(p0, p1);
        }
        /// <summary>
        /// Ons the connection banned.
        /// </summary>
        public override void OnConnectionBanned()
        {
            _agoraService.OnConnectionBanned();
        }
        /// <summary>
        /// Ons the first local audio frame.
        /// </summary>
        /// <param name="p0">P0.</param>
        public override void OnFirstLocalAudioFrame(int p0)
        {
            _agoraService.OnFirstLocalAudioFrame(p0);
        }
        /// <summary>
        /// Ons the first local video frame.
        /// </summary>
        /// <param name="p0">P0.</param>
        /// <param name="p1">P1.</param>
        /// <param name="p2">P2.</param>
        public override void OnFirstLocalVideoFrame(int p0, int p1, int p2)
        {
            _agoraService.OnFirstLocalVideoFrame(p0, p1, p2);
        }
        /// <summary>
        /// Ons the first remote audio frame.
        /// </summary>
        /// <param name="p0">P0.</param>
        /// <param name="p1">P1.</param>
        public override void OnFirstRemoteAudioFrame(int p0, int p1)
        {
            _agoraService.OnFirstRemoteAudioFrame(p0, p1);
        }
        /// <summary>
        /// Ons the first remote video frame.
        /// </summary>
        /// <param name="p0">P0.</param>
        /// <param name="p1">P1.</param>
        /// <param name="p2">P2.</param>
        /// <param name="p3">P3.</param>
        public override void OnFirstRemoteVideoFrame(int p0, int p1, int p2, int p3)
        {
            _agoraService.OnFirstRemoteVideoFrame(p0, p1, p2, p3);
        }
        /// <summary>
        /// Ons the local publish fallback to audio only.
        /// </summary>
        /// <param name="p0">If set to <c>true</c> p0.</param>
        public override void OnLocalPublishFallbackToAudioOnly(bool p0)
        {
            _agoraService.OnLocalPublishFallbackToAudioOnly(p0);
        }
        /// <summary>
        /// Ons the local video stats.
        /// </summary>
        /// <param name="p0">P0.</param>
        public override void OnLocalVideoStats(LocalVideoStats p0)
        {
            _agoraService.OnLocalVideoStats(p0);
        }
        /// <summary>
        /// Ons the media engine load success.
        /// </summary>
        public override void OnMediaEngineLoadSuccess()
        {
            _agoraService.OnMediaEngineLoadSuccess();
        }
        /// <summary>
        /// Ons the media engine start call success.
        /// </summary>
        public override void OnMediaEngineStartCallSuccess()
        {
            _agoraService.OnMediaEngineStartCallSuccess();
        }
        /// <summary>
        /// Ons the microphone enabled.
        /// </summary>
        /// <param name="p0">If set to <c>true</c> p0.</param>
        public override void OnMicrophoneEnabled(bool p0)
        {
            _agoraService.OnMicrophoneEnabled(p0);
        }
        /// <summary>
        /// Ons the network quality.
        /// </summary>
        /// <param name="p0">P0.</param>
        /// <param name="p1">P1.</param>
        /// <param name="p2">P2.</param>
        public override void OnNetworkQuality(int p0, int p1, int p2)
        {
            _agoraService.OnNetworkQuality(p0, p1, p2);
        }
        /// <summary>
        /// Ons the remote audio transport stats.
        /// </summary>
        /// <param name="p0">P0.</param>
        /// <param name="p1">P1.</param>
        /// <param name="p2">P2.</param>
        /// <param name="p3">P3.</param>
        public override void OnRemoteAudioTransportStats(int p0, int p1, int p2, int p3)
        {
            _agoraService.OnRemoteAudioTransportStats(p0, p1, p2, p3);
        }
        /// <summary>
        /// Ons the remote subscribe fallback to audio only.
        /// </summary>
        /// <param name="p0">P0.</param>
        /// <param name="p1">If set to <c>true</c> p1.</param>
        public override void OnRemoteSubscribeFallbackToAudioOnly(int p0, bool p1)
        {
            _agoraService.OnRemoteSubscribeFallbackToAudioOnly(p0, p1);
        }
        /// <summary>
        /// Ons the remote video state changed.
        /// </summary>
        /// <param name="p0">P0.</param>
        /// <param name="p1">P1.</param>
        public override void OnRemoteVideoStateChanged(int p0, int p1, int p2, int p3)
        {
            _agoraService.OnRemoteVideoStateChanged(p0, p1, p2, p3);
        }
        /// <summary>
        /// Ons the remote video stats.
        /// </summary>
        /// <param name="p0">P0.</param>
        public override void OnRemoteVideoStats(RemoteVideoStats p0)
        {
            _agoraService.OnRemoteVideoStats(p0);
        }
        /// <summary>
        /// Ons the remote video transport stats.
        /// </summary>
        /// <param name="p0">P0.</param>
        /// <param name="p1">P1.</param>
        /// <param name="p2">P2.</param>
        /// <param name="p3">P3.</param>
        public override void OnRemoteVideoTransportStats(int p0, int p1, int p2, int p3)
        {
            _agoraService.OnRemoteVideoTransportStats(p0, p1, p2, p3);
        }
        /// <summary>
        /// Ons the request token.
        /// </summary>
        public override void OnRequestToken()
        {
            _agoraService.OnRequestToken();
        }
        /// <summary>
        /// Ons the stream injected status.
        /// </summary>
        /// <param name="p0">P0.</param>
        /// <param name="p1">P1.</param>
        /// <param name="p2">P2.</param>
        public override void OnStreamInjectedStatus(string p0, int p1, int p2)
        {
            _agoraService.OnStreamInjectedStatus(p0, p1, p2);
        }
        /// <summary>
        /// Ons the stream message.
        /// </summary>
        /// <param name="p0">P0.</param>
        /// <param name="p1">P1.</param>
        /// <param name="p2">P2.</param>
        public override void OnStreamMessage(int p0, int p1, byte[] p2)
        {
            _agoraService.OnStreamMessage(p0, p1, p2);
        }
        /// <summary>
        /// Ons the stream message error.
        /// </summary>
        /// <param name="p0">P0.</param>
        /// <param name="p1">P1.</param>
        /// <param name="p2">P2.</param>
        /// <param name="p3">P3.</param>
        /// <param name="p4">P4.</param>
        public override void OnStreamMessageError(int p0, int p1, int p2, int p3, int p4)
        {
            _agoraService.OnStreamMessageError(p0, p1, p2, p3, p4);
        }
        /// <summary>
        /// Ons the stream published.
        /// </summary>
        /// <param name="p0">P0.</param>
        /// <param name="p1">P1.</param>
        public override void OnStreamPublished(string p0, int p1)
        {
            _agoraService.OnStreamPublished(p0, p1);
        }
        /// <summary>
        /// Ons the stream unpublished.
        /// </summary>
        /// <param name="p0">P0.</param>
        public override void OnStreamUnpublished(string p0)
        {
            _agoraService.OnStreamUnpublished(p0);
        }
        /// <summary>
        /// Ons the token privilege will expire.
        /// </summary>
        /// <param name="p0">P0.</param>
        public override void OnTokenPrivilegeWillExpire(string p0)
        {
            _agoraService.OnTokenPrivilegeWillExpire(p0);
        }
        /// <summary>
        /// Ons the transcoding updated.
        /// </summary>
        public override void OnTranscodingUpdated()
        {
            _agoraService.OnTranscodingUpdated();
        }
        /// <summary>
        /// Ons the user enable local video.
        /// </summary>
        /// <param name="p0">P0.</param>
        /// <param name="p1">If set to <c>true</c> p1.</param>
        public override void OnUserEnableLocalVideo(int p0, bool p1)
        {
            _agoraService.OnUserEnableLocalVideo(p0, p1);
        }
        /// <summary>
        /// Ons the user enable video.
        /// </summary>
        /// <param name="p0">P0.</param>
        /// <param name="p1">If set to <c>true</c> p1.</param>
        public override void OnUserEnableVideo(int p0, bool p1)
        {
            _agoraService.OnUserEnableVideo(p0, p1);
        }
        /// <summary>
        /// Ons the user joined.
        /// </summary>
        /// <param name="p0">P0.</param>
        /// <param name="p1">P1.</param>
        public override void OnUserJoined(int p0, int p1)
        {
            _agoraService.OnUserJoined(p0, p1);
        }
        /// <summary>
        /// Ons the user mute audio.
        /// </summary>
        /// <param name="p0">P0.</param>
        /// <param name="p1">If set to <c>true</c> p1.</param>
        public override void OnUserMuteAudio(int p0, bool p1)
        {
            _agoraService.OnUserMuteAudio(p0, p1);
        }
        /// <summary>
        /// Ons the video size changed.
        /// </summary>
        /// <param name="p0">P0.</param>
        /// <param name="p1">P1.</param>
        /// <param name="p2">P2.</param>
        /// <param name="p3">P3.</param>
        public override void OnVideoSizeChanged(int p0, int p1, int p2, int p3)
        {
            _agoraService.OnVideoSizeChanged(p0, p1, p2, p3);
        }
    }
}
