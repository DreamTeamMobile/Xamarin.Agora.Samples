using System;
using System.Threading.Tasks;
using DT.Samples.Agora.Shared;
using UIKit;

namespace DT.Samples.Agora.Rtm.iOS
{
    public partial class MainViewController : UIViewController
    {

        public MainViewController (IntPtr ptr) : base (ptr) 
        {
        
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            OfflineSwitch.On = AgoraRtm.OneToOneMessageType == OneToOneMessageType.Offline;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            Logout();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            View.EndEditing(true);
        }

        partial void DoLoginPress(UIButton sender)
        {
            Login();
        }

        private async Task Login()
        {
            var account = AccountText.Text;
            AgoraRtm.Current = account;

            var token = await AgoraTokenService.GetRtmToken(account);
            AgoraRtm.RtmKit.LoginByToken(token, account, (status) =>
            {
                if (status == Xamarin.Agora.AgoraRtmLoginErrorCode.Ok)
                {
                    InvokeOnMainThread(() =>
                    {
                        AgoraRtm.OneToOneMessageType = OfflineSwitch.On ? OneToOneMessageType.Offline : OneToOneMessageType.Normal;
                        //getting offline messages
                        var rtmDelegate = new RtmDelegate();
                        rtmDelegate.AppendMessage += (user, message) => AgoraRtm.AddOfflineMessage(message, user);
                        AgoraRtm.UpdateKit(rtmDelegate);

                        AgoraRtm.Status = LoginStatus.Online;
                        PerformSegue("mainToTab", null);
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