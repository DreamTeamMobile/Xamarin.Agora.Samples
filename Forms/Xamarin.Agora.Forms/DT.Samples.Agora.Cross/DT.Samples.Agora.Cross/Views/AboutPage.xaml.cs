using System;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace DT.Samples.Agora.Cross.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }

        private void LinkButtonClicked(object sender, System.EventArgs e)
        {
            var button = sender as Button;
            if (Uri.IsWellFormedUriString(button?.Text, UriKind.Absolute))
            {
                Device.OpenUri(new Uri(button?.Text, UriKind.Absolute));
            }
        }
    }
}