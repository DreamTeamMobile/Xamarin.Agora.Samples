using DT.Samples.Agora.Shared;
using DT.Xamarin.Agora;
using System;
using System.Threading.Tasks;
using UIKit;

namespace DT.Samples.Agora.Voice.iOS
{
    public partial class ViewController : UIViewController
    {
        private AgoraRtcEngineKit _agoraKit;
        private const string ChannelName = "drmtm.us";

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            JoinButton.TouchUpInside += OnJoinClicked;
            JoinButton.Hidden = true;

            InitializeAgoraEngine();
            JoinChannel();
        }

        private void OnJoinClicked(object sender, EventArgs e)
        {
            InitializeAgoraEngine();
            JoinChannel();
            ControlButtonsView.Hidden = false;
            JoinButton.Hidden = true;
        }

        private void InitializeAgoraEngine()
        {
            // Initializes the Agora engine with your app ID.
            _agoraKit = AgoraRtcEngineKit.SharedEngineWithAppIdAndDelegate(AgoraTestConstants.AgoraAPI, null);
        }

        private async Task JoinChannel()
        {
            var token = await AgoraTokenService.GetRtcToken(ChannelName);
            _agoraKit.JoinChannelByToken(token, ChannelName, string.Empty, 0, (s, i, k) =>
            {
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
            ControlButtonsView.Hidden = true;
            UIApplication.SharedApplication.IdleTimerDisabled = false;
            JoinButton.Hidden = false;
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