using DT.Xamarin.Agora.Rtm;

namespace DT.Samples.Agora.Rtm.Droid
{
    public class MessageBean
    {
        public string Account { get; set; }
        public RtmMessage Message { get; set; }
        public int Background { get; set; }
        public bool BeSelf { get; set; }
        public string CacheFile { get; set; }

        public MessageBean(string account, RtmMessage message, bool beSelf)
        {
            Message = message;
            BeSelf = beSelf;
            Account = account;
        }
    }
}