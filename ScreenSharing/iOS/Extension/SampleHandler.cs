using System;
using System.Timers;
using CoreFoundation;
using CoreMedia;
using Foundation;
using ReplayKit;

namespace DT.Samples.Agora.ScreenSharing.iOS.Extension
{
    // To handle samples with a subclass of RPBroadcastSampleHandler set the following in the extension's Info.plist file:
    // - RPBroadcastProcessMode should be set to RPBroadcastProcessModeSampleBuffer
    // - NSExtensionPrincipalClass should be set to this class
    public class SampleHandler : RPBroadcastSampleHandler
    {
        private CMSampleBuffer _bufferCopy;
        private long _lastSendTs = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        private Timer _timer;
        private AgoraUploader _agoraUploader;

        protected SampleHandler(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void BroadcastStarted(NSDictionary<NSString, NSObject> setupInfo)
        {
            Console.WriteLine("Agora BroadcastStarted");
            _agoraUploader = new AgoraUploader();
            if (setupInfo.ContainsKey(new NSString("channel")))
            {
                var channel = setupInfo["channel"];
                _agoraUploader.StartBroadcast(channel.ToString());
            }
            else
            {
                _agoraUploader.StartBroadcast("ScreenShare");
            }
            DispatchQueue.MainQueue.DispatchAsync(() =>
            {
                _timer = new Timer(0.1);
                _timer.Elapsed += (s, e) =>
                {
                    var elapse = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - _lastSendTs;
                    if (elapse > 300)
                    {
                        if (_bufferCopy != null)
                        {
                            ProcessSampleBuffer(_bufferCopy, RPSampleBufferType.Video);
                        }
                    }
                };
                _timer.Start();
            });
        }

        public override void BroadcastPaused()
        {
            Console.WriteLine("Agora BroadcastPaused");
            // User has requested to pause the broadcast. Samples will stop being delivered.
        }

        public override void BroadcastResumed()
        {
            Console.WriteLine("Agora BroadcastResumed");
            // User has requested to resume the broadcast. Samples delivery will resume.
        }

        public override void BroadcastFinished()
        {
            Console.WriteLine("Agora BroadcastFinished");
            _timer?.Stop();
            _timer = null;
            _agoraUploader?.StopBroadcast();
        }

        public override void ProcessSampleBuffer(CoreMedia.CMSampleBuffer sampleBuffer, RPSampleBufferType sampleBufferType)
        {
            switch (sampleBufferType)
            {
                case RPSampleBufferType.Video:
                    _bufferCopy = sampleBuffer;
                    _lastSendTs = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    break;
                case RPSampleBufferType.AudioApp:
                    // Handle audio sample buffer for app audio
                    break;
                case RPSampleBufferType.AudioMic:
                    // Handle audio sample buffer for app audio
                    break;
            }
        }
    }
}
