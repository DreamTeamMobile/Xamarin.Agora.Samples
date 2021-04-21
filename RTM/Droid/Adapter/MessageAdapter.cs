using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using DT.Xamarin.Agora.Rtm;

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
            Log.Debug("Adapter", $"getView for index {position}");
            var bean = _messageBeanList[position];
            var message = bean.Message;
            if (bean.BeSelf)
            {
                holder.TextViewSelfName.Text = bean.Account;
            }
            else
            {
                holder.TextViewOtherName.Text = bean.Account;
            }
            //switch (message.MessageType)
            //{
            //    case RtmMessageType.Text:
            //        if (bean.BeSelf)
            //        {
            //            holder.TextViewSelfMsg.Visibility = ViewStates.Visible;
            //            holder.TextViewSelfMsg.Text = message.Text;
            //        }
            //        else
            //        {
            //            holder.TextViewOtherMsg.Visibility = ViewStates.Visible;
            //            holder.TextViewOtherMsg.Text = message.Text;
            //            if (bean.Background != 0)
            //            {
            //                holder.TextViewOtherName.SetBackgroundResource(bean.Background);
            //            }
            //        }

            //        holder.ImageViewSelfImg.Visibility = ViewStates.Gone;
            //        holder.ImageViewOtherImg.Visibility = ViewStates.Gone;
            //        break;
            //    case RtmMessageType.Image:
            //        var imageMessage = message as RtmImageMessage;
            //        var bmp = BitmapFactory.DecodeByteArray(imageMessage.GetThumbnail(), 0, imageMessage.GetThumbnail().Length);
            //        if (bean.BeSelf)
            //        {
            //            holder.ImageViewSelfImg.Visibility = ViewStates.Visible;
            //            holder.ImageViewSelfImg.LayoutParameters.Width = imageMessage.ThumbnailWidth;
            //            holder.ImageViewSelfImg.LayoutParameters.Height = imageMessage.ThumbnailHeight;
            //            holder.ImageViewSelfImg.SetImageBitmap(bmp);
            //        }
            //        else
            //        {
            //            holder.ImageViewOtherImg.Visibility = ViewStates.Visible;
            //            holder.ImageViewOtherImg.LayoutParameters.Width = imageMessage.ThumbnailWidth;
            //            holder.ImageViewOtherImg.LayoutParameters.Height = imageMessage.ThumbnailHeight;
            //            holder.ImageViewOtherImg.SetImageBitmap(bmp);
            //        }

            //        holder.TextViewSelfMsg.Visibility = ViewStates.Gone;
            //        holder.TextViewOtherMsg.Visibility = ViewStates.Gone;
            //        break;
            //    case RtmMessageType.Raw:
            //        var raw = message.GetRawMessage();
            //        if (bean.BeSelf)
            //        {
            //            holder.TextViewSelfMsg.Visibility = ViewStates.Visible;
            //            holder.TextViewSelfMsg.Text = $"Raw[{raw.Length}bytes] {message.Text}";
            //        }
            //        else
            //        {
            //            holder.TextViewOtherMsg.Visibility = ViewStates.Visible;
            //            holder.TextViewOtherMsg.Text = $"Raw[{raw.Length}bytes] {message.Text}";
            //            if (bean.Background != 0)
            //            {
            //                holder.TextViewOtherName.SetBackgroundResource(bean.Background);
            //            }
            //        }

            //        holder.ImageViewSelfImg.Visibility = ViewStates.Gone;
            //        holder.ImageViewOtherImg.Visibility = ViewStates.Gone;
            //        break;
            //}
            
            holder.LayoutRight.Visibility = bean.BeSelf ? ViewStates.Visible : ViewStates.Gone;
            holder.LayoutLeft.Visibility = bean.BeSelf ? ViewStates.Gone : ViewStates.Visible;
        }
    }
}