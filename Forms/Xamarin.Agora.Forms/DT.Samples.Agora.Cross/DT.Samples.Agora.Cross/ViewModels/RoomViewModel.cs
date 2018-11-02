using DT.Samples.Agora.Cross.Views;
using Xamarin.Agora.Full.Forms;
using Xamarin.Forms;

namespace DT.Samples.Agora.Cross.ViewModels
{
    public class RoomViewModel : BaseViewModel
    {
        private IAgoraService _agoraService;

        private bool _isAudioMute;
        public bool IsAudioMute
        {
            get => _isAudioMute;
            set => SetProperty(ref _isAudioMute, value);
        }

        private bool _isVideoMute;
        public bool IsVideoMute
        {
            get => _isVideoMute;
            set => SetProperty(ref _isVideoMute, value);
        }

        private bool _isSpeakerEnabled;
        public bool IsSpeakerEnabled
        {
            get => _isSpeakerEnabled;
            set => SetProperty(ref _isSpeakerEnabled, value);
        }

        private bool _isCameraSwitched;
        public bool IsCameraSwitched
        {
            get => _isCameraSwitched;
            set => SetProperty(ref _isCameraSwitched, value);
        }

        public string Room { get; set; }

        public Command EndSessionCommand { get; }
        public Command AudioMuteCommand { get; }
        public Command VideoMuteCommand { get; }
        public Command SpeakerCommand { get; }
        public Command SwitchCameraCommand { get; }

        public RoomViewModel(string roomName = null)
        {
            Title = roomName;
            Room = roomName;

            EndSessionCommand = new Command(OnEndSession);
            AudioMuteCommand = new Command(OnAudioMute);
            VideoMuteCommand = new Command(OnVideoMute);
            SpeakerCommand = new Command(OnSpeaker);
            SwitchCameraCommand = new Command(SwitchCamera);
        }

        private void SwitchCamera(object param)
        {
            IsCameraSwitched = !IsCameraSwitched;
            _agoraService.ToggleCamera();
        }

        private void OnSpeaker(object param)
        {
            IsSpeakerEnabled = !IsSpeakerEnabled;
            _agoraService.SetSpeakerEnabled(IsSpeakerEnabled);
        }

        private void OnAudioMute(object param)
        {
            IsAudioMute = !IsAudioMute;
            _agoraService.SetAudioMute(IsAudioMute);
        }

        private void OnVideoMute(object param)
        {
            IsVideoMute = !IsVideoMute;
            _agoraService.SetVideoMute(IsVideoMute);

        }

        private void OnEndSession(object param)
        {
            _agoraService.EndSession();
            (Application.Current.MainPage as MainPage).Detail.Navigation.PopAsync();
        }

        public void Init()
        {
            if (_agoraService == null)
            {
                _agoraService = AgoraService.Current;
                _agoraService.JoinChannelSuccess += (uid) => { };
                _agoraService.OnDisconnected += _agoraService_OnDisconnected;
                _agoraService.OnNewStream += _agoraService_OnNewStream;
                _agoraService.StartSession(Room, "988bb3b0b9294430b96e59bb19180ae9");
            }
        }


        void _agoraService_OnNewStream(uint arg1, int arg2, int arg3)
        {
        }


        void _agoraService_OnDisconnected(uint obj)
        {
        }
    }
}
