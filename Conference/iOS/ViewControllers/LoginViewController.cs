using System;
using System.Threading.Tasks;
using UIKit;

namespace DT.Samples.Agora.Conference.iOS
{
    public partial class LoginViewController : UIViewController
    {
        public LoginViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            LoginButton.TouchUpInside += async (s, e) => await Login();
            RtmService.Instance.OnLogin += OnLogin;
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            RtmService.Instance.OnLogin -= OnLogin;
        }

        private async Task Login()
        {
            ErrorLabel.Text = string.Empty;
            var userName = UserNameEdit.Text;
            if (string.IsNullOrEmpty(userName) || userName.Length > 64)
            {
                ErrorLabel.Text = "Invalid user name";
            }
            else
            {
                LoginButton.Enabled = false;
                await RtmService.Instance.Init(userName);
            }
        }

        private void OnLogin(bool success)
        {
            LoginButton.Enabled = true;
            if (success)
            {
                NavigationController.PushViewController(Storyboard.InstantiateViewController("JoinViewController") as JoinViewController, true);
            }
            else
            {
                ErrorLabel.Text = "Could not login";
            }
        }
    }
}