using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using DT.Samples.Agora.Cross.Models;
using DT.Samples.Agora.Cross.ViewModels;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace DT.Samples.Agora.Cross.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoomPage : ContentPage
    {
        RoomViewModel viewModel;

        public RoomPage(RoomViewModel viewModel)
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            BindingContext = this.viewModel = viewModel;
            viewModel.Init();
        }

        public RoomPage() : this(new RoomViewModel("DesignTimeRoom"))
        {
           
        }

        protected override void OnDisappearing()
        {
            viewModel.EndSessionCommand.Execute(false);
            base.OnDisappearing();
        }
    }
}