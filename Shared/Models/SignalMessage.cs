using System;
namespace DT.Samples.Agora.Shared
{
    public enum SignalActionTypes
    {
        HandUp = 1,
        HandDown = 2,
        IncomingCall = 3,
        RejectCall = 4
    }

    public class SignalMessage
    {
        public uint RtcPeerId { get; set; }

        public string RtmUserName { get; set; }

        public SignalActionTypes Action { get; set; }

        public string Data { get; set; }
    }
}
