using System;
using Foundation;
using UIKit;

namespace DT.Samples.Agora.Rtm.iOS
{
    public partial class ChannelViewController : UIViewController
    {
        public ChannelViewController(IntPtr ptr) : base(ptr) 
        { 

        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            View.EndEditing(true);
        }

        partial void DoJoinPressed(UIButton sender)
        {
            var channel = ChannelTextField.Text;

            if (string.IsNullOrEmpty(channel))
                return;

            PerformSegue("channelToChat", NSObject.FromObject(channel));
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            var identifier = segue.Identifier;
            string channel = sender.ToString();

            switch (identifier)
            {
                case "channelToChat":
                    if (segue.DestinationViewController is ChatViewController chatVC)
                    {
                        chatVC.ChannelOrPeerName = channel;
                        chatVC.ChatType = ChatType.Group;
                    }
                    break;
            }
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            View.EndEditing(true);
        }
    }
}