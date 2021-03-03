namespace DT.Samples.Agora.Rtm.Droid
{
    public class MessageBean
    {
        public string Account { get; set; }
        public string Message { get; set; }
        public int Background { get; set; }
        public bool BeSelf { get; set; }

        public MessageBean(string account, string message, bool beSelf)
        {
            Message = message;
            BeSelf = beSelf;
            Account = account;
        }
    }
}