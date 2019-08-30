using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DT.Samples.Agora.Cross
{
    /// <summary>
    /// Agora video view.
    /// </summary>
    public partial class AgoraVideoView : View
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Xamarin.Agora.Full.Forms.AgoraVideoView"/> class.
        /// </summary>
        public AgoraVideoView()
        {
            GUID = Guid.NewGuid();
        }
        /// <summary>
        /// The UIDP roperty.
        /// </summary>
        public static readonly BindableProperty GUIDProperty = BindableProperty.Create(
                                                           "GUID", //Public name to use
                                                           typeof(Guid), //this type
                                                           typeof(AgoraVideoView), //parent type (this control)
                                                           Guid.Empty); //default value
        /// <summary>
        /// Gets or sets the guid.
        /// </summary>
        /// <value>The guid.</value>
        public Guid GUID
        {
            get { return (Guid)GetValue(GUIDProperty); }
            protected set { SetValue(GUIDProperty, value); }
        }

        public static readonly BindableProperty StreamUIDProperty = BindableProperty.Create(
                                                           "StreamUID", //Public name to use
                                                           typeof(uint), //this type
                                                           typeof(AgoraVideoView), //parent type (this control)
                                                           (uint)Consts.UnknownRemoteStreamId); //default value
        /// <summary>
        /// Gets or sets the stream uid.
        /// </summary>
        /// <value>The agora stream uid. <c>1</c> if is video is unknown remote; if local then <c>0</c>.</value>
        public uint StreamUID
        {
            get { return (uint)GetValue(StreamUIDProperty); }
            set { SetValue(StreamUIDProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Xamarin.Agora.Full.Forms.AgoraVideoView"/> stream is
        /// video muted.
        /// </summary>
        /// <value><c>true</c> if is video muted; otherwise, <c>false</c>.</value>
        public bool IsVideoMuted
        {
            get { return (bool)GetValue(IsVideoMutedProperty); }
            set { SetValue(IsVideoMutedProperty, value); }
        }
        /// <summary>
        /// The is video muted property.
        /// </summary>
        public static readonly BindableProperty IsVideoMutedProperty = BindableProperty.Create(
                                                           "IsVideoMuted", //Public name to use
                                                           typeof(bool), //this type
                                                           typeof(AgoraVideoView), //parent type (this control)
                                                           false); //default value
        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Xamarin.Agora.Full.Forms.AgoraVideoView"/> stream is
        /// audio muted.
        /// </summary>
        /// <value><c>true</c> if is audio muted; otherwise, <c>false</c>.</value>
        public bool IsAudioMuted
        {
            get { return (bool)GetValue(IsAudioMutedProperty); }
            set { SetValue(IsAudioMutedProperty, value); }
        }
        /// <summary>
        /// The is audio muted property.
        /// </summary>
        public static readonly BindableProperty IsAudioMutedProperty = BindableProperty.Create(
                                                           "IsAudioMuted", //Public name to use
                                                           typeof(bool), //this type
                                                           typeof(AgoraVideoView), //parent type (this control)
                                                           false); //default value
        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Xamarin.Agora.Full.Forms.AgoraVideoView"/> stream is talking.
        /// </summary>
        /// <value><c>true</c> if is talking; otherwise, <c>false</c>.</value>
        public bool IsTalking
        {
            get { return (bool)GetValue(IsTalkingProperty); }
            set { SetValue(IsTalkingProperty, value); }
        }
        /// <summary>
        /// The is talking property.
        /// </summary>
        public static readonly BindableProperty IsTalkingProperty = BindableProperty.Create(
                                                           "IsTalking", //Public name to use
                                                           typeof(bool), //this type
                                                           typeof(AgoraVideoView), //parent type (this control)
                                                           false); //default value
        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Xamarin.Agora.Full.Forms.AgoraVideoView"/> stream is offline.
        /// </summary>
        /// <value><c>true</c> if is offline; otherwise, <c>false</c>.</value>
        public bool IsOffline
        {
            get { return (bool)GetValue(IsOfflineProperty); }
            set { SetValue(IsOfflineProperty, value); }
        }
        /// <summary>
        /// The is offline property.
        /// </summary>
        public static readonly BindableProperty IsOfflineProperty = BindableProperty.Create(
                                                           "IsOffline", //Public name to use
                                                           typeof(bool), //this type
                                                           typeof(AgoraVideoView), //parent type (this control)
                                                           true); //default value
        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Xamarin.Agora.Full.Forms.AgoraVideoView"/> stream has
        /// video enabled.
        /// </summary>
        /// <value><c>true</c> if is video enabled; otherwise, <c>false</c>.</value>
        public bool IsVideoEnabled
        {
            get { return (bool)GetValue(IsVideoEnabledProperty); }
            set { SetValue(IsVideoEnabledProperty, value); }
        }

        public static readonly BindableProperty IsVideoEnabledProperty = BindableProperty.Create(
                                                           "IsVideoEnabled", //Public name to use
                                                           typeof(bool), //this type
                                                           typeof(AgoraVideoView), //parent type (this control)
                                                           true); //default value   
        /// <summary>
        /// Gets or sets current view rendering mode mode.
        /// </summary>
        /// <value>Fit, Adaptive, Hidden</value>
        public VideoDisplayMode Mode
        {
            get { return (VideoDisplayMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        public static readonly BindableProperty ModeProperty = BindableProperty.Create(
                                                           "Mode", //Public name to use
                                                           typeof(VideoDisplayMode), //this type
                                                           typeof(AgoraVideoView), //parent type (this control)
                                                           VideoDisplayMode.Fit); //default value   
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Xamarin.Agora.Full.Forms.AgoraVideoView"/> will be not removed from page during session, it will render first arrived stream in a session.
        /// </summary>
        /// <value><c>true</c> if is static; otherwise, <c>false</c>.</value>
        public bool IsStatic
        {
            get { return (bool)GetValue(IsStaticProperty); }
            set { SetValue(IsStaticProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Xamarin.Agora.Full.Forms.AgoraVideoView"/> will be not removed from page during session, it will render first arrived stream in a session.
        /// </summary>
        public static readonly BindableProperty IsStaticProperty = BindableProperty.Create(
                                                           "IsStatic", //Public name to use
                                                           typeof(bool), //this type
                                                           typeof(AgoraVideoView), //parent type (this control)
                                                           false); //default value

    }
}

