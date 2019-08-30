using System;
namespace DT.Samples.Agora.Cross
{
    /// <summary>
    /// Agora service.
    /// </summary>
    public interface IAgoraService
    {
        /// <summary>
        /// Gets the implementation platform.
        /// </summary>
        /// <value>The implementation platform.</value>
        string ImplementationPlatform { get; }
        /// <summary>
        /// Occurs when on info update.
        /// </summary>
        event Action<string> OnInfoUpdate;
        /// <summary>
        /// Occurs when join channel success.
        /// </summary>
        event Action<uint> JoinChannelSuccess;
        /// <summary>
        /// Occurs when on disconnected.
        /// </summary>
        event Action<uint> OnDisconnected;
        /// <summary>
        /// Occurs when on new stream.
        /// </summary>
        event Action<uint, int, int> OnNewStream;

        /// <summary>
        /// Starts the session.
        /// </summary>
        /// <param name="sessionId">Session identifier.</param>
        /// <param name="agoraKey">Agora key.</param>
        void StartSession(string sessionId, string agoraKey, string token, VideoAgoraProfile profile = VideoAgoraProfile.Portrait360P, bool swapWidthAndHeight = false, bool webSdkInteroperability = false);
        /// <summary>
        /// Ends the session.
        /// </summary>
        void EndSession();
        /// <summary>
        /// Toggles the camera.
        /// </summary>
        void ToggleCamera();
        /// <summary>
        /// Sets the speaker enabled.
        /// </summary>
        /// <param name="speaker">If set to <c>true</c> speaker.</param>
        void SetSpeakerEnabled(bool speaker);
        /// <summary>
        /// Sets the audio mute.
        /// </summary>
        /// <param name="isMute">If set to <c>true</c> is mute.</param>
        void SetAudioMute(bool isMute);
        /// <summary>
        /// Sets the video mute.
        /// </summary>
        /// <param name="isMute">If set to <c>true</c> is mute.</param>
        void SetVideoMute(bool isMute);
    }
}
