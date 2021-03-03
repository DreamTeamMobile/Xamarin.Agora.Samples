namespace DT.Samples.Agora.Rtm.iOS
{
    public class Message
    {
        public string UserId;
        public string Text;

        public Message(string userId, string text)
        {
            UserId = userId;
            Text = text;
        }
    }
}