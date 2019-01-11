using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using DT.Samples.Agora.Cross.Models;
using DT.Samples.Agora.Cross.ViewModels;

using Xamarin.Forms;

namespace DT.Samples.Agora.Cross.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MultiRoomPage : ContentPage
    {
        MultiRoomViewModel viewModel;

        public MultiRoomPage(MultiRoomViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
            viewModel.Init();
        }

        public MultiRoomPage()
        {
            InitializeComponent();
            viewModel = new MultiRoomViewModel("NoRoom");
            BindingContext = viewModel;
        }

        protected override void OnDisappearing()
        {
            viewModel.EndSessionCommand.Execute(false);
            base.OnDisappearing();
        }
    }
}
