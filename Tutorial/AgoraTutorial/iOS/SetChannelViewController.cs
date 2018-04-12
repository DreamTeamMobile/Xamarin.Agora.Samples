using System;
using Foundation;
using UIKit;

namespace AgoraTutorial.iOS
{
    public partial class SetChannelViewController : UIViewController
    {
        public SetChannelViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.		
        }

        partial void StartCallClick(NSObject sender)
        {
            if (string.IsNullOrWhiteSpace(channelNameTextField.Text))
            {
                channelName.Text = "Please input channel name to proceed";
            }
            else
            {
                var storyboard = UIStoryboard.FromName("Main", null);
                var controller = storyboard.InstantiateViewController("VideoCallView");
                VideoCallViewController.Channel = channelNameTextField.Text;
                this.ShowViewController(controller, this);
            }
        }
    }
}
