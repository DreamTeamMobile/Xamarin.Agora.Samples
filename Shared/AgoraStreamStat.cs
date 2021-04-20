using System;
namespace DT.Samples.Agora.Shared
{
    public class AgoraStreamStat
    {
        public ulong Bitrate;
        public ulong FrameRate;
        public bool IsSent;

        public AgoraStreamStat() { }

        public AgoraStreamStat(ulong bitrate, ulong framerate, bool sent)
        {
            Bitrate = bitrate;
            FrameRate = framerate;
            IsSent = sent;
        }
    }
}
