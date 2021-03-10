using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace DT.Samples.Agora.Rtm.Droid
{
    public class MyViewHolder : RecyclerView.ViewHolder
    {
        public TextView TextViewOtherName;
        public TextView TextViewOtherMsg;
        public ImageView ImageViewOtherImg;
        public TextView TextViewSelfName;
        public TextView TextViewSelfMsg;
        public ImageView ImageViewSelfImg;
        public RelativeLayout LayoutLeft;
        public RelativeLayout LayoutRight;

        public MyViewHolder(View itemView) : base(itemView)
        {
            TextViewOtherName = itemView.FindViewById<TextView>(Resource.Id.item_name_l);
            TextViewOtherMsg = itemView.FindViewById<TextView>(Resource.Id.item_msg_l);
            ImageViewOtherImg = itemView.FindViewById<ImageView>(Resource.Id.item_img_l);
            TextViewSelfName = itemView.FindViewById<TextView>(Resource.Id.item_name_r);
            TextViewSelfMsg = itemView.FindViewById<TextView>(Resource.Id.item_msg_r);
            ImageViewSelfImg = itemView.FindViewById<ImageView>(Resource.Id.item_img_r);
            LayoutLeft = itemView.FindViewById<RelativeLayout>(Resource.Id.item_layout_l);
            LayoutRight = itemView.FindViewById<RelativeLayout>(Resource.Id.item_layout_r);
        }
    }
}