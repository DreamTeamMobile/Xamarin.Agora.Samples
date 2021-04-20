using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DT.Samples.Agora.Cross.ViewModels;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace DT.Samples.Agora.Cross.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MultiRoomPage : ContentPage
    {
        private MultiRoomViewModel _viewModel;

        public MultiRoomPage(MultiRoomViewModel viewModel)
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            BindingContext = _viewModel = viewModel;
            _viewModel.Init();
        }

        public MultiRoomPage() : this(new MultiRoomViewModel("DesignTimeRoom"))
        {
        }

        protected override void OnDisappearing()
        {
            _viewModel.EndSessionCommand.Execute(false);
            base.OnDisappearing();
        }
    }
}
