using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using DT.Samples.Agora.Cross.Models;
using DT.Samples.Agora.Cross.ViewModels;

namespace DT.Samples.Agora.Cross.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoomPage : ContentPage
    {
        RoomViewModel viewModel;

        public RoomPage(RoomViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
            viewModel.Init();


        }

        public RoomPage()
        {
            InitializeComponent();
            viewModel = new RoomViewModel("NoRoom");
            BindingContext = viewModel;
        }
    }
}