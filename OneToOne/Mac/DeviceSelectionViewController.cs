using System;
using System.Collections.Generic;
using AppKit;
using Foundation;
using Xamarin.Agora.Mac;

namespace DT.Samples.Agora.OneToOne.Mac
{
    [Register(nameof(DeviceSelectionViewController))]
    public partial class DeviceSelectionViewController : NSViewController
    {

        private readonly List<AgoraRtcDeviceInfo> connectedRecordingDevices = new List<AgoraRtcDeviceInfo>();
        private readonly List<AgoraRtcDeviceInfo> connectedPlaybackDevices = new List<AgoraRtcDeviceInfo>();
        private readonly List<AgoraRtcDeviceInfo> connectedVideoCaptureDevices = new List<AgoraRtcDeviceInfo>();
        private AgoraRtcEngineKit _agoraKit;

        public DeviceSelectionViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            PreferredContentSize = new CoreGraphics.CGSize(500, 250);
        }

        private void LoadDevicesInPopUpButtons()
        {
            FillInDeviceSelection(MediaDeviceType.AudioRecording, microphoneSelection, connectedRecordingDevices);
            FillInDeviceSelection(MediaDeviceType.AudioPlayout, speakerSelection, connectedPlaybackDevices);
            FillInDeviceSelection(MediaDeviceType.VideoCapture, cameraSelection, connectedVideoCaptureDevices);
        }

        private void FillInDeviceSelection(MediaDeviceType deviceType, NSPopUpButton deviceSelection, List<AgoraRtcDeviceInfo> deviceMap)
        {
            deviceSelection.RemoveAllItems();
            deviceMap.Clear();
            var devices = _agoraKit.EnumerateDevices(deviceType);
            foreach (var device in devices)
            {
                deviceSelection.AddItem(device.DeviceName);
                deviceMap.Add(device);
            }
        }

        internal void SetAgoraKit(AgoraRtcEngineKit agoraKit)
        {
            _agoraKit = agoraKit;
            _agoraKit.StateChanged += OnAgoraDeviceTypeStateChanged;
            LoadDevicesInPopUpButtons();
        }

        partial void didClickConfirmButton(Foundation.NSObject sender)
        {
            var recordingDevice = connectedRecordingDevices[(int)microphoneSelection.IndexOfSelectedItem]?.DeviceId;
            if (!String.IsNullOrEmpty(recordingDevice))
            {
                _agoraKit.SetDevice(MediaDeviceType.AudioRecording, recordingDevice);
            }

            var playbackDevice = connectedPlaybackDevices[(int)speakerSelection.IndexOfSelectedItem]?.DeviceId;
            if (!String.IsNullOrEmpty(playbackDevice))
            {
                _agoraKit.SetDevice(MediaDeviceType.AudioPlayout, playbackDevice);
            }
            var videoDevice = connectedVideoCaptureDevices[(int)cameraSelection.IndexOfSelectedItem]?.DeviceId;
            if (!String.IsNullOrEmpty(videoDevice))
            {
                _agoraKit.SetDevice(MediaDeviceType.VideoCapture, recordingDevice);
            }
            DismissViewController(this);
        }

        private void OnAgoraDeviceTypeStateChanged(object sender, StateChangedEventArgs e)
        {
            BeginInvokeOnMainThread(LoadDevicesInPopUpButtons);
        }

    }
}
