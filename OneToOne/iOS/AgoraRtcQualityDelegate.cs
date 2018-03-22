using DT.Xamarin.Agora;

namespace DT.Samples.Agora.OneToOne.iOS
{
    public class AgoraRtcQualityDelegate : AgoraRtcEngineDelegate
    {
        private JoinViewController _controller;

        public AgoraRtcQualityDelegate(JoinViewController controller) : base()
        {
            _controller = controller;
        }

        public override void LastmileQuality(AgoraRtcEngineKit engine, Quality quality)
        {
            _controller.LastmileQuality(engine, quality);
        }
    }
}
