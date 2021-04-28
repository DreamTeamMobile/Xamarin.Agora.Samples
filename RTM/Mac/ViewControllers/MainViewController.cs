using System;
using Foundation;
using AppKit;
using DT.Samples.Agora.Rtm.Mac.Delegates;
using DT.Samples.Agora.Shared;
using System.Threading.Tasks;

namespace DT.Samples.Agora.Rtm.Mac
{
	public partial class MainViewController : NSViewController
	{
		public MainViewController(IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override void ViewWillAppear()
        {
            base.ViewWillAppear();
            Logout();
        }

        public override void ViewWillDisappear()
        {
            base.ViewWillDisappear();
        }

        public override void PrepareForSegue(NSStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            if (segue.DestinationController is PeerChannelViewController peerChannelVC)
            {
                peerChannelVC.OnGoBack += (vc) => vc.View.Window.ContentViewController = this;
            }
        }

        partial void DoLoginPress(NSButton sender)
        {
            Login();
        }

        private async Task Login()
        {
            var account = AccountText.StringValue;
            AgoraRtm.Current = account;

            var token = await AgoraTokenService.GetRtmToken(account);
            AgoraRtm.RtmKit.LoginByToken(token, account, (status) =>
            {
                if (status == Xamarin.Agora.AgoraRtmLoginErrorCode.Ok)
                {
                    InvokeOnMainThread(() =>
                    {
                        AgoraRtm.OneToOneMessageType = OfflineSwitch.State == NSCellStateValue.On
                            ? OneToOneMessageType.Offline
                            : OneToOneMessageType.Normal;
                        //getting offline messages
                        var rtmDelegate = new RtmDelegate();
                        rtmDelegate.AppendMessage += (user, message) => AgoraRtm.AddOfflineMessage(message, user);
                        AgoraRtm.UpdateKit(rtmDelegate);

                        AgoraRtm.Status = LoginStatus.Online;
                        PerformSegue("mainToTab", this);
                    });
                }
            });
        }

        public void Logout()
        {
            if (AgoraRtm.Status != LoginStatus.Online)
                return;

            AgoraRtm.RtmKit.LogoutWithCompletion((status) =>
            {
                if (status == Xamarin.Agora.AgoraRtmLogoutErrorCode.Ok)
                {
                    AgoraRtm.Status = LoginStatus.Offline;
                }
            });
        }
    }
}
