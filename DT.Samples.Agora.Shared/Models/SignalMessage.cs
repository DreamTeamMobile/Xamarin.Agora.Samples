using System;
namespace DT.Samples.Agora.Shared
{
    public enum SignalActionTypes
    {
        HandUp = 1,
        HandDown = 2
    }

    public class SignalMessage
    {
        public uint PeerId { get; set; }

        public SignalActionTypes Action { get; set; }
    }
}
