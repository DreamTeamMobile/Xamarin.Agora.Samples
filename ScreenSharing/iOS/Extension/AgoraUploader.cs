using System;
using System.Threading.Tasks;
using CoreAnimation;
using CoreFoundation;
using CoreGraphics;
using CoreMedia;
using DT.Samples.Agora.Shared;
using DT.Xamarin.Agora;
using Foundation;
using UIKit;

namespace DT.Samples.Agora.ScreenSharing.iOS.Extension
{
    public static class ExtensionConstants
    {
        public static int SCREEN_SHARE_BROADCASTER_UID_MIN = 1001;
        public static int SCREEN_SHARE_BROADCASTER_UID_MAX = 2000;

        public static int SCREEN_SHARE_UID = new Random().Next(SCREEN_SHARE_BROADCASTER_UID_MIN, SCREEN_SHARE_BROADCASTER_UID_MAX);
    }

    public class AgoraUploader
    {
        private readonly CGSize VideoDimension;
        private const uint AudioSampleRate = 48000;
        private const uint AudioChannels = 2;

        private AgoraRtcEngineKit _sharedAgoraEngine;

        public AgoraUploader()
        {
            var screenSize = UIScreen.MainScreen.CurrentMode.Size;
            var boundingSize = new CGSize(720, 1280);
            var mW = boundingSize.Width / screenSize.Width;
            var mH = boundingSize.Height / screenSize.Height;
            if (mH < mW)
            {
                boundingSize.Width = boundingSize.Height / screenSize.Height * screenSize.Width;
            }
            else if (mW < mH)
            {
                boundingSize.Height = boundingSize.Width / screenSize.Width * screenSize.Height;
            }
            VideoDimension = new CGSize(boundingSize);

            InitAgora();
        }

        private void InitAgora()
        {
            _sharedAgoraEngine = AgoraRtcEngineKit.SharedEngineWithAppIdAndDelegate(AgoraTestConstants.AgoraAPI, null);
            _sharedAgoraEngine.SetChannelProfile(ChannelProfile.LiveBroadcasting);
            _sharedAgoraEngine.SetClientRole(ClientRole.Broadcaster);


            _sharedAgoraEngine.EnableVideo();
            _sharedAgoraEngine.SetExternalVideoSource(true, useTexture: true, pushMode: true);
            var videoConfig = new AgoraVideoEncoderConfiguration(VideoDimension,
                                                                VideoFrameRate.VideoFrameRateFps24,
                                                                3000,
                                                                VideoOutputOrientationMode.Adaptative);
            _sharedAgoraEngine.SetVideoEncoderConfiguration(videoConfig);
            _sharedAgoraEngine.SetAudioProfile(AudioProfile.MusicStandardStereo, AudioScenario.Default);


            _sharedAgoraEngine.EnableExternalAudioSourceWithSampleRate(AudioSampleRate,
                                                                       AudioChannels);

            _sharedAgoraEngine.MuteAllRemoteVideoStreams(true);
            _sharedAgoraEngine.MuteAllRemoteAudioStreams(true);
        }

        public void StartBroadcast(string channel)
        {
            Task.Run(async () =>
            {
                var token = await AgoraTokenService.GetRtcToken(channel);
                _sharedAgoraEngine.JoinChannelByToken(token, channel, string.Empty, (uint)ExtensionConstants.SCREEN_SHARE_UID, new AgoraRtcChannelMediaOptions());
            });
        }

        public void SendVideBuffer(CMSampleBuffer sampleBuffer)
        {            
            var videoFrame = sampleBuffer.GetImageBuffer();
            var rotation = 0;
            var orientationAttachment = sampleBuffer.GetAttachment<NSNumber>((CFString)ReplayKit.RPBroadcastSampleHandler.VideoSampleOrientationKey.ToString(), out CMAttachmentMode mode);
            switch(orientationAttachment.UInt32Value)
            {
                case 1: //up
                case 2: //upMirrored
                    rotation = 0;
                    break;
                case 3: //down
                case 4: //downMirrored
                    rotation = 180;
                    break;
                case 8: //left
                case 5: //leftMirrored
                    rotation = 90;
                    break;
                case 6: //right
                case 7: //rightMirrored
                    rotation = 270;
                    break;
            }
            var time = new CMTime((long)CAAnimation.CurrentMediaTime(), 1000);
            var frame = new AgoraVideoFrame
            {
                Format = 12,
                Time = time,
                TextureBuf = videoFrame.Handle,
                Rotation = rotation
            };
            _sharedAgoraEngine.PushExternalVideoFrame(frame);
        }

        public void SendAudioAppBuffer(CMSampleBuffer sampleBuffer)
        {

        }

        public void SendAudioMicBuffer(CMSampleBuffer sampleBuffer)
        {

        }

        public void StopBroadcast()
        {
            _sharedAgoraEngine.LeaveChannel(null);
        }
    }
}
