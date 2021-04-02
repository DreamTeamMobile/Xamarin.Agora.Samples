using Foundation;
using UIKit;

namespace DT.Samples.Agora.ScreenSharing.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            UINavigationBar.Appearance.BarTintColor = Theme.BarTintColor;
            UINavigationBar.Appearance.TintColor = Theme.TintColor;
            UINavigationBar.Appearance.TitleTextAttributes = new UIStringAttributes()
            {
                ForegroundColor = Theme.TitleTextColor
            };
            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
            return true;
        }
    }
}
