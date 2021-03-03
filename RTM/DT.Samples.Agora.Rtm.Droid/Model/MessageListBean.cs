using System.Collections.Generic;

namespace DT.Samples.Agora.Rtm.Droid
{
    public class MessageListBean
    {
        public string AccountOther { get; set; }
        public List<MessageBean> MessageBeanList { get; set; }

        public MessageListBean(string account, List<MessageBean> messageBeanList)
        {
            AccountOther = account;
            MessageBeanList = messageBeanList;
        }
    }
}