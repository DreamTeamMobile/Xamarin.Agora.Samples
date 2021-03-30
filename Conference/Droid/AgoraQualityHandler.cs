using DT.Xamarin.Agora;

namespace DT.Samples.Agora.Conference.Droid
{
    public class AgoraQualityHandler : IRtcEngineEventHandler
    {
        private JoinActivity _context;

        public AgoraQualityHandler(JoinActivity activity)
        {
            _context = activity;
        }

        public override void OnLastmileQuality(int p0)
        {
            _context.OnLastmileQuality(p0);
        }
    }
}
