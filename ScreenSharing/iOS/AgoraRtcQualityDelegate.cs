using DT.Xamarin.Agora;

namespace DT.Samples.Agora.ScreenSharing.iOS
{
    public class AgoraRtcQualityDelegate : AgoraRtcEngineDelegate
    {
        private JoinViewController _controller;

        public AgoraRtcQualityDelegate(JoinViewController controller) : base()
        {
            _controller = controller;
        }

        public override void LastmileQuality(AgoraRtcEngineKit engine, NetworkQuality quality)
        {
            _controller.LastmileQuality(engine, quality);
        }
    }
}
