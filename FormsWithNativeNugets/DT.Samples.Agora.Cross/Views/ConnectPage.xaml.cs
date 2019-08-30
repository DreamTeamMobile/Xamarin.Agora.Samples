using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DT.Samples.Agora.Cross.ViewModels;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace DT.Samples.Agora.Cross.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectPage : ContentPage
    {
        private ConnectViewModel _viewModel;
        public ConnectPage()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            BindingContext = _viewModel = new ConnectViewModel();
            RoomName.Keyboard = Keyboard.Create(KeyboardFlags.None);
        }

        private void Handle_OneToOne_Clicked(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_viewModel.RoomName))
                Navigation.PushAsync(new RoomPage(new RoomViewModel(_viewModel.RoomName)));
        }

        private void Handle_Group_Clicked(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_viewModel.RoomName))
                Navigation.PushAsync(new MultiRoomPage(new MultiRoomViewModel(_viewModel.RoomName)));
        }
    }
}