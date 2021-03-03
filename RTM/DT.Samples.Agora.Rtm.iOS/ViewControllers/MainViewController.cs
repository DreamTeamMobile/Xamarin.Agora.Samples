using System;
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

        private void Login()
        {
            var account = AccountText.Text;
            AgoraRtm.Current = account;

            AgoraRtm.RtmKit.LoginByToken(null, account, (status) =>
            {
                if (status == Xamarin.Agora.AgoraRtmLoginErrorCode.Ok)
                {
                    InvokeOnMainThread(() =>
                    {
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