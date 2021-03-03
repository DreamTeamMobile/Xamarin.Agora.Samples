using System.Collections.Generic;
using Android;
using Java.Util;

namespace DT.Samples.Agora.Rtm.Droid
{
    public static class MessageUtil
    {
        public const int MaxInputNameLength = 64;
        public const int ActivityResultConnAborted = 1;
        public const string IntentExtraIsPeerMode = "chatMode";
        public const string IntentExtraUserId = "userId";
        public const string IntentExtraTargetName = "targetName";

        public static readonly List<int> ColorList = new List<int> 
        {
                Resource.Drawable.shape_circle_black,
                Resource.Drawable.shape_circle_blue,
                Resource.Drawable.shape_circle_pink,
                Resource.Drawable.shape_circle_pink_dark,
                Resource.Drawable.shape_circle_yellow,
                Resource.Drawable.shape_circle_red
        };

        public static Random RandomGenerator = new Random();

        private static List<MessageListBean> _messageListBeanList = new List<MessageListBean>();

        public static void AddMessageListBeanList(MessageListBean messageListBean)
        {
            _messageListBeanList.Add(messageListBean);
        }

        // clean up list on logout
        public static void CleanMessageListBeanList()
        {
            _messageListBeanList.Clear();
        }

        public static MessageListBean GetExistMessageListBean(string accountOther)
        {
            int ret = ExistMessageListBean(accountOther);
            if (ret > -1)
            {
                var item = _messageListBeanList[ret];
                _messageListBeanList.Remove(item);

                return item;
            }
            return null;
        }

        // return existing list position
        private static int ExistMessageListBean(string userId)
        {
            int size = _messageListBeanList.Count;
            for (int i = 0; i < size; i++)
            {
                if (_messageListBeanList[i].AccountOther.Equals(userId))
                {
                    return i;
                }
            }
            return -1;
        }

        public static void AddMessageBean(string account, string msg)
        {
            MessageBean messageBean = new MessageBean(account, msg, false);
            int ret = ExistMessageListBean(account);
            if (ret == -1)
            {
                // account not exist new messagelistbean
                messageBean.Background = ColorList[RandomGenerator.NextInt(ColorList.Count)];

                List<MessageBean> messageBeanList = new List<MessageBean>
                {
                    messageBean
                };

                _messageListBeanList.Add(new MessageListBean(account, messageBeanList));
            }
            else
            {
                // account exist get messagelistbean
                MessageListBean bean = _messageListBeanList[ret];

                List<MessageBean> messageBeanList = bean.MessageBeanList;
                if (messageBeanList.Count > 0)
                {
                    messageBean.Background =  messageBeanList[0].Background;
                }
                else
                {
                    messageBean.Background = ColorList[RandomGenerator.NextInt(ColorList.Count)];
                }

                messageBeanList.Add(messageBean);
                bean.MessageBeanList = messageBeanList;
            }
        }
    }
}