using DT.Xamarin.Agora;

namespace DT.Samples.Agora.Rtm.iOS
{
    public class Message
    {
        public string UserId { get; set; }
        public AgoraRtmMessage RtmMessage { get; set; }

        public Message(string userId, AgoraRtmMessage message)
        {
            UserId = userId;
            RtmMessage = message;
        }
    }
}