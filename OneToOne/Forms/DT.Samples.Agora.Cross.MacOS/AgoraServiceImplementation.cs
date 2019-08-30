using System;
using System.Collections.Generic;
using System.Linq;
using AppKit;
using DT.Samples.Agora.Cross.MacOS;
using Xamarin.Agora.Mac;
using Xamarin.Forms;

[assembly: Dependency(typeof(AgoraServiceImplementation))]
namespace DT.Samples.Agora.Cross.MacOS
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
        public string ImplementationPlatform { get { return "MacOS"; } }
        /// <summary>
        /// The max volume.
        /// </summary>
        public const int MaxVolume = 255;
        /// <summary>
        /// The data stream identifier.
        /// </summary>
        private AgoraRtcDelegate _agoraDelegate;
        private AgoraRtcEngineKit _agoraEngine;
        private nuint _myId = 0;
        private readonly List<AgoraVideoViewHolder<NSView>> _containers = new List<AgoraVideoViewHolder<NSView>>();
        private readonly List<nuint> _knownStreams = new List<nuint> { };
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
            _agoraEngine.StopPreview();
            _agoraEngine.SetupLocalVideo(null);
            _agoraEngine.StopAudioMixing();
            _agoraEngine.StopAudioRecording();
            _agoraEngine.LeaveChannel(null);
            _agoraEngine.Dispose();
            AgoraRtcEngineKit.Destroy();
            _agoraEngine = null;
        }
        /// <summary>
        /// Toggles the camera.
        /// </summary>
        public void ToggleCamera()
        {
            //_agoraEngine.SwitchCamera();
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
            //_agoraEngine.SetEnableSpeakerphone(speaker);
        }

        private VideoProfile GetVideoProfile(VideoAgoraProfile profile)
        {
            switch (profile)
            {
                case VideoAgoraProfile.Landscape1080P:
                    return VideoProfile.Landscape1080P;
                case VideoAgoraProfile.Landscape720P:
                    return VideoProfile.Landscape720P;
                case VideoAgoraProfile.Landscape480P:
                    return VideoProfile.Landscape480P;
                case VideoAgoraProfile.Landscape360P:
                    return VideoProfile.Landscape360P;
                case VideoAgoraProfile.Landscape240P:
                    return VideoProfile.Landscape240P;
                case VideoAgoraProfile.Landscape120P:
                    return VideoProfile.Landscape120P;
                case VideoAgoraProfile.Landscape4K:
                    return VideoProfile.Landscape4K;
                case VideoAgoraProfile.Portrait4K:
                    return VideoProfile.Portrait4K;
                case VideoAgoraProfile.Portrait120P:
                    return VideoProfile.Portrait120P;
                case VideoAgoraProfile.Portrait240P:
                    return VideoProfile.Portrait240P;
                case VideoAgoraProfile.Portrait360P:
                    return VideoProfile.Portrait360P;
                case VideoAgoraProfile.Portrait480P:
                    return VideoProfile.Portrait480P;
                case VideoAgoraProfile.Portrait720P:
                    return VideoProfile.Portrait720P;
                case VideoAgoraProfile.Portrait1080P:
                    return VideoProfile.Portrait1080P;
            }
            return VideoProfile.Default;
        }

        /// <summary>
        /// Starts the session.
        /// </summary>
        /// <param name="sessionId">Session identifier.</param>
        /// <param name="agoraAPI">Agora API key.</param>
        public virtual void StartSession(string sessionId, string agoraAPI, string token, VideoAgoraProfile profile = VideoAgoraProfile.Portrait360P, bool swapWidthAndHeight = false, bool webSdkInteroperability = false)
        {
            _knownStreams.Add(_myId);
            _agoraDelegate = new AgoraRtcDelegate(this);
            _agoraEngine = AgoraRtcEngineKit.SharedEngineWithAppId(agoraAPI, _agoraDelegate);
            _agoraEngine.EnableWebSdkInteroperability(webSdkInteroperability);
            //_agoraEngine.SetChannelProfile(ChannelProfile.Communication);
            _agoraEngine.EnableLocalVideo(true);
            //_agoraEngine.SwitchCamera();
            _agoraEngine.EnableVideo();
            _agoraEngine.SetVideoProfile(GetVideoProfile(profile), swapWidthAndHeight);
            //_agoraEngine.SetVideoProfile(VideoProfile.Portrait360P, false);
            // if you do not specify the uid, we will generate the uid for you
            _agoraEngine.JoinChannelByToken(token, sessionId, null, 0, JoiningCompleted);
        }

        public virtual void SetupView(AgoraVideoViewHolder<NSView> holder)
        {
            if (!_knownStreams.Contains(holder.StreamUID) && holder.StreamUID != Consts.UnknownRemoteStreamId)
                return;
            if (!_containers.Any(a => a.GUID == holder.GUID))
                _containers.Add(holder);
            if (holder.StreamUID != Consts.UnknownRemoteStreamId)
                SetupVideo(holder.GUID);
        }

        internal void OnUserOffline(AgoraRtcEngineKit engine, nuint uid, UserOfflineReason reason)
        {
            var id = (uint)uid;
            _knownStreams.Remove(id);
            var toClear = _containers.Where(a => a.StreamUID == id && a.IsStatic).ToList();
            var toRemove = _containers.Where(a => a.StreamUID == id && !a.IsStatic).ToList();
            foreach (var container in toRemove)
            {
                container.NativeView.RemoveFromSuperview();
                _containers.Remove(container);
            }
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
        public virtual void SetupVideo(nuint uid)
        {
            var containers = _containers.Where(a => a.StreamUID == uid);
            if (containers.Count() == 0)
                return;
            foreach (var container in containers)
            {
                var canvas = new AgoraRtcVideoCanvas()
                {
                    View = container.NativeView,
                    RenderMode = (VideoRenderMode)(int)container.VideoView.Mode,
                    Uid = uid
                };
                container.VideoView.IsOffline = false;
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
        }
        /// <summary>
        /// Setups the video.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public virtual void SetupVideo(Guid id)
        {
            var container = _containers.FirstOrDefault(a => a.GUID == id);
            if (container == null)
                return;
            var canvas = new AgoraRtcVideoCanvas()
            {
                View = container.NativeView,
                RenderMode = (VideoRenderMode)(int)container.VideoView.Mode,
                Uid = container.StreamUID
            };
            container.VideoView.IsOffline = false;
            if (container.StreamUID == 0 || container.StreamUID == _myId)
            {
                _agoraEngine.SetupLocalVideo(canvas);
                _agoraEngine.StartPreview();
            }
            else
            {
                _agoraEngine.SetupRemoteVideo(canvas);
            }
        }

        /// <summary>
        /// Firsts the remote video decoded of uid.
        /// </summary>
        /// <param name="engine">Engine.</param>
        /// <param name="uid">Uid.</param>
        /// <param name="size">Size.</param>
        /// <param name="elapsed">Elapsed.</param>
        public virtual void FirstRemoteVideoDecodedOfUid(AgoraRtcEngineKit engine, nuint uid, CoreGraphics.CGSize size, nint elapsed)
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
                    OnNewStream(id, (int)size.Width, (int)size.Height);
                }
            }
        }

        /// <summary>
        /// Dids the video muted.
        /// </summary>
        /// <param name="engine">Engine.</param>
        /// <param name="muted">If set to <c>true</c> muted.</param>
        /// <param name="uid">Uid.</param>
        public virtual void OnUserMuteVideo(AgoraRtcEngineKit engine, nuint uid, bool muted)
        {
            var id = (uint)uid;
            foreach (var holder in _containers.Where(a => a.StreamUID == id))
            {
                holder.VideoView.IsVideoMuted = muted;
            }
        }

        public virtual void OnUserEnabledVideo(AgoraRtcEngineKit engine, nuint uid, bool enabled)
        {
            var id = (uint)uid;
            foreach (var holder in _containers.Where(a => a.StreamUID == id))
            {
                holder.VideoView.IsVideoEnabled = enabled;
            }
        }

        public virtual void OnUserMuteAudio(AgoraRtcEngineKit engine, nuint uid, bool muted)
        {
            var id = (uint)uid;
            foreach (var holder in _containers.Where(a => a.StreamUID == id))
            {
                holder.VideoView.IsAudioMuted = muted;
            }
        }

        public virtual void OnUserJoined(AgoraRtcEngineKit engine, nuint uid)
        {
            var id = (uint)uid;
            foreach (var holder in _containers.Where(a => a.StreamUID == id))
            {
                holder.VideoView.IsOffline = false;
            }
        }

        private void JoiningCompleted(Foundation.NSString arg1, nuint arg2, nint arg3)
        {
            _myId = arg2;
            JoinChannelSuccess((uint)_myId);
        }
    }
}
