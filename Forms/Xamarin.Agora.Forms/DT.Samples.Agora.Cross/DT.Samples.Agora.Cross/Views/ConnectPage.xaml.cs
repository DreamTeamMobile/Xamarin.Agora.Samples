using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DT.Samples.Agora.Cross.ViewModels;

namespace DT.Samples.Agora.Cross.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectPage : ContentPage
    {
        ConnectViewModel viewModel;

        public ConnectPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ConnectViewModel();
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new RoomPage(new RoomViewModel(viewModel.RoomName)));
        }
    }
}