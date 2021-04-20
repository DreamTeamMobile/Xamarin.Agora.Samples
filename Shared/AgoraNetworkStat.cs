using System;
namespace DT.Samples.Agora.Shared
{
    public class AgoraNetworkStat
    {
        public AgoraQuality Rx;
        public AgoraQuality Tx;

        public AgoraNetworkStat() { }

        public AgoraNetworkStat(int rx, int tx)
        {
            Rx = (AgoraQuality)rx;
            Tx = (AgoraQuality)tx;
        }
    }
}
