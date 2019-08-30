using DT.Samples.Agora.Cross.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using NavigationPage = Xamarin.Forms.NavigationPage;

namespace DT.Samples.Agora.Cross.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        private Dictionary<int, NavigationPage> _menuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            MasterBehavior = MasterBehavior.Popover;
            _menuPages.Add((int)MenuItemType.Call, (NavigationPage)Detail);
        }

        public async Task NavigateFromMenu(int id)
        {
            if (Device.RuntimePlatform == Device.macOS && id == (int)MenuItemType.About)
            {
                // see https://github.com/xamarin/Xamarin.Forms/issues/4300 and other similar mac-related crashes with navigation
                DisplayAlert("About",
@"Xamarin.Forms sample for Agora SDK

Proudly presented by Agora and DreamTeam Mobile

https://agora.io

https://drmtm.us", "OK");
                return;

            }
            if (!_menuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Call:
                        _menuPages.Add(id, new NavigationPage(new ConnectPage()));
                        break;
                    case (int)MenuItemType.About:
                        _menuPages.Add(id, new NavigationPage(new AboutPage()));
                        break;
                }
            }

            var newPage = _menuPages[id];
            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;
                if (Device.RuntimePlatform == Device.Android)
                {
                    await Task.Delay(100); //auto-hide menu on Android
                }
                IsPresented = false;
            }
        }
    }
}