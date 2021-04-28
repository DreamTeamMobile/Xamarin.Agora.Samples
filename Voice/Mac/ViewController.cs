using System;
using System.Threading.Tasks;
using AppKit;
using AVFoundation;
using DT.Samples.Agora.Shared;
using Xamarin.Agora.Mac;

namespace DT.Samples.Agora.Voice.Mac
{
    public partial class ViewController : NSViewController
    {
        private AgoraRtcEngineKit _agoraKit;
        private const string ChannelName = "drmtm.us";

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            JoinButton.Activated += OnJoinClicked;
            SetControlsVisible(true);

            CheckPermissionAndRunAgoraEngine();
        }

        private void OnJoinClicked(object sender, EventArgs e)
        {
            CheckPermissionAndRunAgoraEngine();
            SetControlsVisible(false);
        }

        private void CheckPermissionAndRunAgoraEngine()
        {
            SetError(string.Empty);
            AVCaptureDevice.RequestAccessForMediaType(AVAuthorizationMediaType.Audio, (accessGranted) =>
            {
                BeginInvokeOnMainThread(() =>
                {
                    if (accessGranted)
                    {
                        _agoraKit = AgoraRtcEngineKit.SharedEngineWithAppId(AgoraTestConstants.AgoraAPI, (error) =>
                        {
                            SetError($"Could not init agora. {error}");
                            SetControlsVisible(false);
                        });
                        JoinChannel();
                        SetControlsVisible(true);
                    }
                    else
                    {
                        SetError("Permission denied. Please allow access to microphone");
                        SetControlsVisible(false);
                    }
                });
            });
        }

        private async Task JoinChannel()
        {
            var token = await AgoraTokenService.GetRtcToken(ChannelName);
            _agoraKit.JoinChannelByToken(token, ChannelName, string.Empty, 0, (s, i, k) =>
            {
                _agoraKit.EnableAudio();
                var devices = _agoraKit.EnumerateDevices(MediaDeviceType.AudioRecording);
                _agoraKit.SetDevice(MediaDeviceType.AudioRecording, devices[0].DeviceId);
            });
        }

        private void SetError(string error)
        {
            ErrorLabel.StringValue = error;
        }

        private void SetControlsVisible(bool visible)
        {
            ControlButtonsView.Hidden = !visible;
            JoinButton.Hidden = visible;
        }

        partial void DidClickHangUpButton(NSButton sender)
        {
            LeaveChannel();
        }

        private void LeaveChannel()
        {
            _agoraKit?.LeaveChannel(null);
            SetControlsVisible(false);
        }

        partial void DidClickMuteButton(NSButton sender)
        {
            var isOn = sender.State == NSCellStateValue.On;
            _agoraKit.MuteLocalAudioStream(isOn);
            sender.Image = NSImage.ImageNamed(isOn ? "muteButtonSelected" : "muteButton");
        }

        partial void DidClickDeviceButton(NSButton sender)
        {
            var deviceSelectionViewController = this.Storyboard.InstantiateControllerWithIdentifier("DeviceSelectionViewController") as DeviceSelectionViewController;
            this.PresentViewControllerAsSheet(deviceSelectionViewController);
            deviceSelectionViewController.SetAgoraKit(this._agoraKit);
        }
    }
}
