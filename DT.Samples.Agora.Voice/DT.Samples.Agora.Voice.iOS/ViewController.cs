using DT.Samples.Agora.Shared;
using DT.Xamarin.Agora;
using System;
using UIKit;

namespace DT.Samples.Agora.Voice.iOS
{
    public partial class ViewController : UIViewController
    {
        private AgoraRtcEngineKit _agoraKit;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitializeAgoraEngine();
            JoinChannel();
        }

        private void InitializeAgoraEngine()
        {
            // Initializes the Agora engine with your app ID.
            _agoraKit = AgoraRtcEngineKit.SharedEngineWithAppIdAndDelegate(AgoraTestConstants.AgoraAPI, null);
        }

        private void JoinChannel()
        {
            _agoraKit.JoinChannelByToken(AgoraTestConstants.Token, "voiceDemoChannel1", string.Empty, 0, (s, i, k) =>
            {
                // Joined channel "voiceDemoChannel1"
                _agoraKit.SetEnableSpeakerphone(true);
                UIApplication.SharedApplication.IdleTimerDisabled = true;
            });
        }

        partial void DidClickHangUpButton(UIButton sender)
        {
            LeaveChannel();
        }

        private void LeaveChannel()
        {
            _agoraKit.LeaveChannel(null);
            HideControlButtons();
            UIApplication.SharedApplication.IdleTimerDisabled = false;
        }

        private void HideControlButtons()
        {
            ControlButtonsView.Hidden = true;
        }

        partial void DidClickMuteButton(UIButton sender)
        {
            sender.Selected = !sender.Selected;
            _agoraKit.MuteLocalAudioStream(sender.Selected);
        }

        partial void DidClickSwitchSpeakerButton(UIButton sender)
        {
            sender.Selected = !sender.Selected;
            // Enables/Disables the audio playback route to the speakerphone.
            //
            // This method sets whether the audio is routed to the speakerphone or earpiece. After calling this method, the SDK returns the onAudioRouteChanged callback to indicate the changes.
            _agoraKit.SetEnableSpeakerphone(sender.Selected);
        }
    }
}