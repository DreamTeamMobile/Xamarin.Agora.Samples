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

        public MessageListBean(string account, ChatManager chatManager)
        {
            AccountOther = account;
            MessageBeanList = new List<MessageBean>();
            var messageList = chatManager.GetAllOfflineMessages(account);
            messageList.ForEach(m =>
            {
                var bean = new MessageBean(account, m, false);
                MessageBeanList.Add(bean);
            });
        }
    }
}