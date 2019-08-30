using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.Views;
using Android.Widget;
using DT.Samples.Agora.Cross.Droid;
using DT.Xamarin.Agora;
using DT.Xamarin.Agora.Video;
using Xamarin.Forms;

[assembly: Dependency(typeof(AgoraServiceImplementation))]
namespace DT.Samples.Agora.Cross.Droid
{

    /// <summary>
    /// Agora service implementation.
    /// </summary>
    public class AgoraServiceImplementation : IAgoraService
    {
        /// <summary>
        /// Gets the implementation platform.
        /// </summary>
        /// <value>The implementation platform.</value>
        public string ImplementationPlatform { get { return "Android"; } }

        /// <summary>
        /// The max volume.
        /// </summary>
        public const int MaxVolume = 255;
        private RtcEngine _agoraEngine;

        private AgoraRtcEngineEventHandler _agoraHandler;
        private readonly List<AgoraVideoViewHolder<FrameLayout>> _containers = new List<AgoraVideoViewHolder<FrameLayout>>();
        private uint _myId = 0;
        private readonly List<uint> _knownStreams = new List<uint> { };

        /// <summary>
        /// Occurs when info update.
        /// </summary>
        public event Action<string> OnInfoUpdate = delegate { };
        /// <summary>
        /// Occurs when joined.
        /// </summary>
        public event Action<uint> JoinChannelSuccess = delegate { };
        /// <summary>
        /// Occurs when disconnected.
        /// </summary>
        public event Action<uint> OnDisconnected = delegate { };
        /// <summary>
        /// Occurs when new stream.
        /// </summary>
        public event Action<uint, int, int> OnNewStream = delegate { };

        /// <summary>
        /// Ends the session.
        /// </summary>
        public virtual void EndSession()
        {
            _containers.Clear();
            _knownStreams.Clear();
            _myId = 0;
            _agoraEngine.SetupRemoteVideo(null);
            _agoraEngine.StopPreview();
            _agoraEngine.SetupLocalVideo(null);
            _agoraEngine.StopAudioRecording();
            _agoraEngine.LeaveChannel();
            _agoraEngine.Dispose();
            _agoraEngine = null;
            RtcEngine.Destroy();
        }

        /// <summary>
        /// Sets the audio mute.
        /// </summary>
        /// <param name="isMute">If set to <c>true</c> is mute.</param>
        public void SetAudioMute(bool isMute)
        {
            _agoraEngine?.MuteLocalAudioStream(isMute);
        }

        /// <summary>
        /// Sets the video mute.
        /// </summary>
        /// <param name="isMute">If set to <c>true</c> is mute.</param>
        public void SetVideoMute(bool isMute)
        {
            _agoraEngine?.MuteLocalVideoStream(isMute);
        }

        /// <summary>
        /// Sets the speaker enabled.
        /// </summary>
        /// <param name="speaker">If set to <c>true</c> speaker.</param>
        public void SetSpeakerEnabled(bool speaker)
        {
            _agoraEngine?.SetEnableSpeakerphone(speaker);
            if (!speaker)
            {
                var localAudioManager = (AudioManager)global::Android.App.Application.Context.GetSystemService(Context.AudioService);
                localAudioManager.Mode = Mode.Normal;
                localAudioManager.BluetoothScoOn = true;
                localAudioManager.StartBluetoothSco();
                localAudioManager.Mode = Mode.InCall;
            }
        }

        private VideoEncoderConfiguration GetVideoConfig(VideoAgoraProfile profile)
        {
            switch(profile)
            {
               case VideoAgoraProfile.Landscape720P:
                    return new VideoEncoderConfiguration(VideoEncoderConfiguration.VD1280x720, VideoEncoderConfiguration.FRAME_RATE.FrameRateFps15, VideoEncoderConfiguration.StandardBitrate, VideoEncoderConfiguration.ORIENTATION_MODE.OrientationModeFixedLandscape);
                case VideoAgoraProfile.Landscape480P:
                    return new VideoEncoderConfiguration(VideoEncoderConfiguration.VD840x480, VideoEncoderConfiguration.FRAME_RATE.FrameRateFps15, VideoEncoderConfiguration.StandardBitrate, VideoEncoderConfiguration.ORIENTATION_MODE.OrientationModeFixedLandscape);
                case VideoAgoraProfile.Landscape360P:
                    return new VideoEncoderConfiguration(VideoEncoderConfiguration.VD480x360, VideoEncoderConfiguration.FRAME_RATE.FrameRateFps15, VideoEncoderConfiguration.StandardBitrate, VideoEncoderConfiguration.ORIENTATION_MODE.OrientationModeFixedLandscape);
                case VideoAgoraProfile.Landscape240P:
                    return new VideoEncoderConfiguration(VideoEncoderConfiguration.VD320x240, VideoEncoderConfiguration.FRAME_RATE.FrameRateFps15, VideoEncoderConfiguration.StandardBitrate, VideoEncoderConfiguration.ORIENTATION_MODE.OrientationModeFixedLandscape);
                case VideoAgoraProfile.Landscape120P:
                    return new VideoEncoderConfiguration(VideoEncoderConfiguration.VD160x120, VideoEncoderConfiguration.FRAME_RATE.FrameRateFps15, VideoEncoderConfiguration.StandardBitrate, VideoEncoderConfiguration.ORIENTATION_MODE.OrientationModeFixedLandscape);
                case VideoAgoraProfile.Portrait120P:
                    return new VideoEncoderConfiguration(VideoEncoderConfiguration.VD160x120, VideoEncoderConfiguration.FRAME_RATE.FrameRateFps15, VideoEncoderConfiguration.StandardBitrate, VideoEncoderConfiguration.ORIENTATION_MODE.OrientationModeFixedPortrait);
                case VideoAgoraProfile.Portrait240P:
                    return new VideoEncoderConfiguration(VideoEncoderConfiguration.VD320x240, VideoEncoderConfiguration.FRAME_RATE.FrameRateFps15, VideoEncoderConfiguration.StandardBitrate, VideoEncoderConfiguration.ORIENTATION_MODE.OrientationModeFixedPortrait);
                case VideoAgoraProfile.Portrait360P:
                    return new VideoEncoderConfiguration(VideoEncoderConfiguration.VD480x360, VideoEncoderConfiguration.FRAME_RATE.FrameRateFps15, VideoEncoderConfiguration.StandardBitrate, VideoEncoderConfiguration.ORIENTATION_MODE.OrientationModeFixedPortrait);
                case VideoAgoraProfile.Portrait480P:
                    return new VideoEncoderConfiguration(VideoEncoderConfiguration.VD840x480, VideoEncoderConfiguration.FRAME_RATE.FrameRateFps15, VideoEncoderConfiguration.StandardBitrate, VideoEncoderConfiguration.ORIENTATION_MODE.OrientationModeFixedPortrait);
                case VideoAgoraProfile.Portrait720P:
                    return new VideoEncoderConfiguration(VideoEncoderConfiguration.VD1280x720, VideoEncoderConfiguration.FRAME_RATE.FrameRateFps15, VideoEncoderConfiguration.StandardBitrate, VideoEncoderConfiguration.ORIENTATION_MODE.OrientationModeFixedPortrait);
            }
            return new VideoEncoderConfiguration(VideoEncoderConfiguration.VD480x360, VideoEncoderConfiguration.FRAME_RATE.FrameRateFps15, VideoEncoderConfiguration.StandardBitrate, VideoEncoderConfiguration.ORIENTATION_MODE.OrientationModeFixedPortrait);
        }

        /// <summary>
        /// Starts the session.
        /// </summary>
        /// <param name="sessionId">Session identifier.</param>
        /// <param name="agoraAPI">Agora API key.</param>
        public void StartSession(string sessionId, string agoraAPI, string token, VideoAgoraProfile profile = VideoAgoraProfile.Portrait360P, bool swapWidthAndHeight = false, bool webSdkInteroperability = false)
        {
            _knownStreams.Add(_myId);
            _agoraHandler = new AgoraRtcEngineEventHandler(this);
            _agoraEngine = RtcEngine.Create(global::Android.App.Application.Context, agoraAPI, _agoraHandler);
            _agoraEngine.EnableWebSdkInteroperability(webSdkInteroperability);
            _agoraEngine.SetChannelProfile(Constants.ChannelProfileCommunication);
            //_agoraEngine.SetVideoProfile(Constants.VideoProfile360p, false); <2.3.0
            _agoraEngine.SetVideoEncoderConfiguration(GetVideoConfig(profile));
            _agoraEngine.EnableLocalVideo(true);
            _agoraEngine.EnableVideo();
            _agoraEngine.SwitchCamera();

            _agoraEngine.SetEnableSpeakerphone(true);
            // if you do not specify the uid, we will generate the uid for you
            _agoraEngine.JoinChannel(token, sessionId, string.Empty, 0);
        }

        /// <summary>
        /// On the join channel success.
        /// </summary>
        /// <param name="channel">Channel.</param>
        /// <param name="uid">Uid.</param>
        /// <param name="elapsed">Elapsed.</param>
        public virtual void OnJoinChannelSuccess(string channel, int uid, int elapsed)
        {
            _myId = (uint)uid;
            if (_agoraEngine != null)
            {
                _agoraEngine.SetEnableSpeakerphone(true);
            }
            JoinChannelSuccess(_myId);
        }

        /// <summary>
        /// Toggles the camera.
        /// </summary>
        public void ToggleCamera()
        {
            if (_agoraEngine != null)
                _agoraEngine.SwitchCamera();
        }

        /// <summary>
        /// On the first remote video decoded.
        /// </summary>
        /// <param name="uid">Uid.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <param name="elapsed">Elapsed.</param>
        public virtual void OnFirstRemoteVideoDecoded(int uid, int width, int height, int elapsed)
        {
            var id = (uint)uid;
            _knownStreams.Add(id);
            if (!_containers.Any(a => a.StreamUID == id))
            {
                if (_containers.Any(a => a.StreamUID == Consts.UnknownRemoteStreamId))
                {
                    var viewHolder = _containers.First(a => a.StreamUID == Consts.UnknownRemoteStreamId);
                    viewHolder.StreamUID = id;
                    viewHolder.VideoView.IsOffline = false;
                }
                else
                {
                    OnNewStream((uint)uid, width, height);
                }
            }
        }

        public virtual void SetupView(AgoraVideoViewHolder<FrameLayout> holder)
        {
            if (!_knownStreams.Contains(holder.StreamUID) && holder.StreamUID != Consts.UnknownRemoteStreamId)
                return;
            if (!_containers.Any(a => a.GUID == holder.GUID))
                _containers.Add(holder);
            if (holder.StreamUID != Consts.UnknownRemoteStreamId)
                SetupVideo(holder.GUID);
        }

        internal void OnUserOffline(int uid, int reason)
        {
            var id = (uint)uid;
            _knownStreams.Remove(id);
            var toClear = _containers.Where(a => a.StreamUID == id && a.IsStatic).ToList();
            var toRemove = _containers.Where(a => a.StreamUID == id && !a.IsStatic).ToList();
            global::Android.App.Application.SynchronizationContext.Post(_ =>
            {
                foreach (var container in toRemove)
                {
                    container.NativeView.RemoveAllViews();
                    _containers.Remove(container);
                }
            }, null);
            foreach (var container in toClear)
            {
                container.VideoView.IsOffline = true;
                container.VideoView.IsAudioMuted = false;
                container.VideoView.IsVideoMuted = false;
                container.VideoView.StreamUID = container.VideoView.StreamUID == Consts.UnknownLocalStreamId ? Consts.UnknownLocalStreamId : Consts.UnknownRemoteStreamId;
            }
            OnDisconnected(id);
        }

        /// <summary>
        /// Setups the video.
        /// </summary>
        /// <param name="uid">Uid.</param>
        public virtual void SetupVideo(uint uid)
        {
            global::Android.App.Application.SynchronizationContext.Post(_ => {
            var containers = _containers.Where(a => a.StreamUID == uid).ToArray();
            if (containers.Length == 0)
                return;
                foreach (var container in containers)
                {
                    var layout = RtcEngine.CreateRendererView(container.NativeView.Context);
                    layout.SetZOrderMediaOverlay(true);
                    container.NativeView.AddView(layout);
                    var canvas = new VideoCanvas(layout, (int)container.VideoView.Mode, (int)uid);
                    container.VideoView.IsOffline = false;
                    if (_agoraEngine != null)
                    {
                        if (uid == 0 || uid == _myId)
                        {
                            _agoraEngine.SetupLocalVideo(canvas);
                            _agoraEngine.StartPreview();
                        }
                        else
                        {
                            _agoraEngine.SetupRemoteVideo(canvas);
                        }
                    }
                    container.NativeView.Visibility = ViewStates.Visible;
                }
            }, null);
        }

        public virtual void SetupVideo(Guid id)
        {
            global::Android.App.Application.SynchronizationContext.Post(_ =>
            {
                var container = _containers.FirstOrDefault(a => a.GUID == id);
                if (container == null)
                    return;
                var layout = RtcEngine.CreateRendererView(container.NativeView.Context);
                layout.SetZOrderMediaOverlay(true);
                container.NativeView.AddView(layout);
                var canvas = new VideoCanvas(layout, (int)container.VideoView.Mode, (int)container.StreamUID);
                container.VideoView.IsOffline = false;
                if (container.StreamUID == 0 || container.StreamUID == _myId)
                {
                    _agoraEngine?.SetupLocalVideo(canvas);
                    _agoraEngine?.StartPreview();
                }
                else
                {
                    _agoraEngine?.SetupRemoteVideo(canvas);
                }
                container.NativeView.Visibility = ViewStates.Visible;
            }, null);
        }

        /// <summary>
        /// On the user mute video.
        /// </summary>
        /// <param name="uid">Uid.</param>
        /// <param name="muted">If set to <c>true</c> muted.</param>
        public virtual void OnUserMuteVideo(int uid, bool muted)
        {
            var id = (uint)uid;
            foreach (var holder in _containers.Where(a => a.StreamUID == id))
            {
                holder.VideoView.IsVideoMuted = muted;
            }
        }
        /// <summary>
        /// On the connection interrupted.
        /// </summary>
        public virtual void OnConnectionInterrupted()
        {
        }
        /// <summary>
        /// On the connection lost.
        /// </summary>
        public virtual void OnConnectionLost()
        {
        }
        /// <summary>
        /// On the error.
        /// </summary>
        /// <param name="err">Error.</param>
        public virtual void OnError(int err)
        {
        }
        /// <summary>
        /// On the lastmile quality.
        /// </summary>
        /// <param name="quality">Quality.</param>
        public virtual void OnLastmileQuality(int quality)
        {
        }
        /// <summary>
        /// On the rejoin channel success.
        /// </summary>
        /// <param name="channel">Channel.</param>
        /// <param name="uid">Uid.</param>
        /// <param name="elapsed">Elapsed.</param>
        public virtual void OnRejoinChannelSuccess(string channel, int uid, int elapsed)
        {
        }
        /// <summary>
        /// On the warning.
        /// </summary>
        /// <param name="warn">Warn.</param>
        public virtual void OnWarning(int warn)
        {
        }
        /// <summary>
        /// On the leave channel.
        /// </summary>
        /// <param name="stats">Stats.</param>
        public virtual void OnLeaveChannel(IRtcEngineEventHandler.RtcStats stats)
        {
        }
        /// <summary>
        /// On the rtc stats.
        /// </summary>
        /// <param name="stats">Stats.</param>
        public virtual void OnRtcStats(IRtcEngineEventHandler.RtcStats stats)
        {
        }
        /// <summary>
        /// On the video stopped.
        /// </summary>
        public virtual void OnVideoStopped()
        {
        }
        /// <summary>
        /// On the active speaker.
        /// </summary>
        /// <param name="uid">Uid.</param>
        public virtual void OnActiveSpeaker(int uid)
        {
        }
        /// <summary>
        /// On the API call executed.
        /// </summary>
        /// <param name="error">Error.</param>
        /// <param name="api">API.</param>
        /// <param name="result">Result.</param>
        public virtual void OnApiCallExecuted(int error, string api, string result)
        {
        }

        /// <summary>
        /// On the audio effect finished.
        /// </summary>
        /// <param name="soundId">Sound identifier.</param>
        public virtual void OnAudioEffectFinished(int soundId)
        {
        }

        /// <summary>
        /// On the audio mixing finished.
        /// </summary>
        public virtual void OnAudioMixingFinished()
        {
        }

        /// <summary>
        /// On the audio quality.
        /// </summary>
        /// <param name="uid">Uid.</param>
        /// <param name="quality">Quality.</param>
        /// <param name="delay">Delay.</param>
        /// <param name="lost">Lost.</param>
        public virtual void OnAudioQuality(int uid, int quality, short delay, short lost)
        {
        }

        /// <summary>
        /// On the audio route changed.
        /// </summary>
        /// <param name="routing">Routing.</param>
        public virtual void OnAudioRouteChanged(int routing)
        {
        }

        /// <summary>
        /// On the audio volume indication.
        /// </summary>
        /// <param name="speakers">Speakers.</param>
        /// <param name="totalVolume">Total volume.</param>
        public virtual void OnAudioVolumeIndication(IRtcEngineEventHandler.AudioVolumeInfo[] speakers, int totalVolume)
        {
        }
        /// <summary>
        /// On the camera focus area changed.
        /// </summary>
        /// <param name="rect">Rect.</param>
        public virtual void OnCameraFocusAreaChanged(Rect rect)
        {
        }

        /// <summary>
        /// On the camera ready.
        /// </summary>
        public virtual void OnCameraReady()
        {
        }

        /// <summary>
        /// On the client role changed.
        /// </summary>
        /// <param name="oldRole">Old role.</param>
        /// <param name="newRole">New role.</param>
        public virtual void OnClientRoleChanged(int oldRole, int newRole)
        {
        }

        /// <summary>
        /// On the connection banned.
        /// </summary>
        public virtual void OnConnectionBanned()
        {
        }

        /// <summary>
        /// On the first local audio frame.
        /// </summary>
        /// <param name="elapsed">Elapsed.</param>
        public virtual void OnFirstLocalAudioFrame(int elapsed)
        {
        }

        /// <summary>
        /// On the first local video frame.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <param name="elapsed">Elapsed.</param>
        public virtual void OnFirstLocalVideoFrame(int width, int height, int elapsed)
        {
        }

        /// <summary>
        /// On the first remote audio frame.
        /// </summary>
        /// <param name="uid">Uid.</param>
        /// <param name="elapsed">Elapsed.</param>
        public virtual void OnFirstRemoteAudioFrame(int uid, int elapsed)
        {
        }

        /// <summary>
        /// On the first remote video frame.
        /// </summary>
        /// <param name="uid">Uid.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <param name="elapsed">Elapsed.</param>
        public virtual void OnFirstRemoteVideoFrame(int uid, int width, int height, int elapsed)
        {
        }

        /// <summary>
        /// On the local publish fallback to audio only.
        /// </summary>
        /// <param name="isFallbackOrRecover">If set to <c>true</c> is fallback or recover.</param>
        public virtual void OnLocalPublishFallbackToAudioOnly(bool isFallbackOrRecover)
        {
        }

        /// <summary>
        /// On the local video stat.
        /// </summary>
        /// <param name="sentBitrate">Sent bitrate.</param>
        /// <param name="sentFrameRate">Sent frame rate.</param>
        public virtual void OnLocalVideoStat(int sentBitrate, int sentFrameRate)
        {
        }

        /// <summary>
        /// On the local video stats.
        /// </summary>
        /// <param name="stats">Stats.</param>
        public virtual void OnLocalVideoStats(IRtcEngineEventHandler.LocalVideoStats stats)
        {
        }

        /// <summary>
        /// On the media engine load success.
        /// </summary>
        public virtual void OnMediaEngineLoadSuccess()
        {
        }

        /// <summary>
        /// On the media engine start call success.
        /// </summary>
        public virtual void OnMediaEngineStartCallSuccess()
        {
        }

        /// <summary>
        /// On the microphone enabled.
        /// </summary>
        /// <param name="enabled">If set to <c>true</c> enabled.</param>
        public virtual void OnMicrophoneEnabled(bool enabled)
        {
        }

        /// <summary>
        /// On the network quality.
        /// </summary>
        /// <param name="uid">Uid.</param>
        /// <param name="txQuality">Tx quality.</param>
        /// <param name="rxQuality">Rx quality.</param>
        public virtual void OnNetworkQuality(int uid, int txQuality, int rxQuality)
        {
        }

        /// <summary>
        /// On the remote audio transport stats.
        /// </summary>
        /// <param name="uid">Uid.</param>
        /// <param name="delay">Delay.</param>
        /// <param name="lost">Lost.</param>
        /// <param name="rxKBitRate">Rx KB it rate.</param>
        public virtual void OnRemoteAudioTransportStats(int uid, int delay, int lost, int rxKBitRate)
        {
        }

        /// <summary>
        /// On the remote subscribe fallback to audio only.
        /// </summary>
        /// <param name="uid">Uid.</param>
        /// <param name="isFallbackOrRecover">If set to <c>true</c> is fallback or recover.</param>
        public virtual void OnRemoteSubscribeFallbackToAudioOnly(int uid, bool isFallbackOrRecover)
        {
        }

        /// <summary>
        /// On the remote video stat.
        /// </summary>
        /// <param name="uid">Uid.</param>
        /// <param name="delay">Delay.</param>
        /// <param name="receivedBitrate">Received bitrate.</param>
        /// <param name="receivedFrameRate">Received frame rate.</param>
        public virtual void OnRemoteVideoStat(int uid, int delay, int receivedBitrate, int receivedFrameRate)
        {
        }

        /// <summary>
        /// On the remote video state changed.
        /// </summary>
        /// <param name="uid">Uid.</param>
        /// <param name="state">State.</param>
        public virtual void OnRemoteVideoStateChanged(int uid, int state, int p2, int p3)
        {
        }

        /// <summary>
        /// On the remote video stats.
        /// </summary>
        /// <param name="stats">Stats.</param>
        public virtual void OnRemoteVideoStats(IRtcEngineEventHandler.RemoteVideoStats stats)
        {
        }

        /// <summary>
        /// On the remote video transport stats.
        /// </summary>
        /// <param name="uid">Uid.</param>
        /// <param name="delay">Delay.</param>
        /// <param name="lost">Lost.</param>
        /// <param name="rxKBitRate">Rx KB it rate.</param>
        public virtual void OnRemoteVideoTransportStats(int uid, int delay, int lost, int rxKBitRate)
        {
        }

        /// <summary>
        /// On the request token.
        /// </summary>
        public virtual void OnRequestToken()
        {
        }

        /// <summary>
        /// On the stream injected status.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="uid">Uid.</param>
        /// <param name="status">Status.</param>
        public virtual void OnStreamInjectedStatus(string url, int uid, int status)
        {
        }

        /// <summary>
        /// On the stream message.
        /// </summary>
        /// <param name="uid">Uid.</param>
        /// <param name="streamId">Stream identifier.</param>
        /// <param name="data">Data.</param>
        public virtual void OnStreamMessage(int uid, int streamId, byte[] data)
        {
        }

        /// <summary>
        /// On the stream message error.
        /// </summary>
        /// <param name="uid">Uid.</param>
        /// <param name="streamId">Stream identifier.</param>
        /// <param name="error">Error.</param>
        /// <param name="missed">Missed.</param>
        /// <param name="cached">Cached.</param>
        public virtual void OnStreamMessageError(int uid, int streamId, int error, int missed, int cached)
        {
        }

        /// <summary>
        /// On the stream published.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="error">Error.</param>
        public virtual void OnStreamPublished(string url, int error)
        {
        }

        /// <summary>
        /// On the stream unpublished.
        /// </summary>
        /// <param name="url">URL.</param>
        public virtual void OnStreamUnpublished(string url)
        {
        }

        /// <summary>
        /// On the token privilege will expire.
        /// </summary>
        /// <param name="token">Token.</param>
        public virtual void OnTokenPrivilegeWillExpire(string token)
        {
        }

        /// <summary>
        /// On the transcoding updated.
        /// </summary>
        public virtual void OnTranscodingUpdated()
        {
        }

        /// <summary>
        /// On the user enable local video.
        /// </summary>
        /// <param name="uid">Uid.</param>
        /// <param name="enabled">If set to <c>true</c> enabled.</param>
        public virtual void OnUserEnableLocalVideo(int uid, bool enabled)
        {
        }

        /// <summary>
        /// On the user enable video.
        /// </summary>
        /// <param name="uid">Uid.</param>
        /// <param name="enabled">If set to <c>true</c> enabled.</param>
        public virtual void OnUserEnableVideo(int uid, bool enabled)
        {
            var id = (uint)uid;
            foreach (var holder in _containers.Where(a => a.StreamUID == id))
            {
                holder.VideoView.IsVideoEnabled = enabled;
            }
        }

        /// <summary>
        /// On the user joined.
        /// </summary>
        /// <param name="uid">Uid.</param>
        /// <param name="elapsed">Elapsed.</param>
        public virtual void OnUserJoined(int uid, int elapsed)
        {
            var id = (uint)uid;
            foreach (var holder in _containers.Where(a => a.StreamUID == id))
            {
                holder.VideoView.IsOffline = false;
            }
        }

        /// <summary>
        /// Ons the user mute audio.
        /// </summary>
        /// <param name="uid">Uid.</param>
        /// <param name="muted">If set to <c>true</c> muted.</param>
        public virtual void OnUserMuteAudio(int uid, bool muted)
        {
            var id = (uint)uid;
            foreach (var holder in _containers.Where(a => a.StreamUID == id))
            {
                holder.VideoView.IsAudioMuted = muted;
            }
        }

        /// <summary>
        /// On the video size changed.
        /// </summary>
        /// <param name="uid">Uid.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <param name="rotation">Rotation.</param>
        public virtual void OnVideoSizeChanged(int uid, int width, int height, int rotation)
        {
        }
    }
}
