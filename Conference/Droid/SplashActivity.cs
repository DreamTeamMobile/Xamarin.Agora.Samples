using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Support.V7.App;

namespace DT.Samples.Agora.Conference.Droid
{
    [Activity(Theme = "@style/DT.Theme.Splash", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(new Intent(Application.Context, typeof(LoginActivity)));
        }

        public override void OnBackPressed() { }
    }
}
