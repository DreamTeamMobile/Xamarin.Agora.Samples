using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;

namespace DT.Samples.Agora.Rtm.Droid
{
    public class MessageAdapter : RecyclerView.Adapter
    {
        private List<MessageBean> _messageBeanList;
        private LayoutInflater _inflater;

        public override int ItemCount => _messageBeanList.Count;

        public MessageAdapter(Context context, List<MessageBean> messageBeanList)
        {
            _inflater = ((Activity)context).LayoutInflater;
            this._messageBeanList = messageBeanList;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = _inflater.Inflate(Resource.Layout.MsgItemLayout, parent, false);
            MyViewHolder holder = new MyViewHolder(view);
            return holder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            SetupView(holder as MyViewHolder, position);
        }

        private void SetupView(MyViewHolder holder, int position)
        {

            MessageBean bean = _messageBeanList[position];
            if (bean.BeSelf)
            {
                holder.TextViewSelfName.Text = bean.Account;
                holder.TextViewSelfMsg.Text = bean.Message;
            }
            else
            {
                holder.TextViewOtherName.Text = bean.Account;
                holder.TextViewOtherMsg.Text = bean.Message;
                if (bean.Background != 0)
                {
                    holder.TextViewOtherName.SetBackgroundResource(bean.Background);
                }
            }

            holder.LayoutRight.Visibility = bean.BeSelf ? ViewStates.Visible : ViewStates.Gone;
            holder.LayoutLeft.Visibility = bean.BeSelf ? ViewStates.Gone : ViewStates.Visible;
        }
    }
}