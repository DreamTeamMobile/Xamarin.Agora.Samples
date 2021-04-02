using System;
using System.Linq;
using DT.Samples.Agora.Shared;
using DT.Samples.Agora.Shared.Helpers;
using UIKit;
using Foundation;

namespace DT.Samples.Agora.ScreenSharing.iOS
{
    public partial class SettingsViewController : UIViewController
    {
        protected SettingsViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationController.NavigationBarHidden = false;
            var profiles = VideoProfiles.Get();
            var profilesModel = new ProfilesModel(profiles);
            profilesModel.ItemSelected += UpdateProfileValues;
            ProfilePicker.Model = profilesModel;
            var selectedProfile = profiles.First(profile => profile.Id == AgoraSettings.Current.Profile);
            ProfilePicker.Select(profiles.IndexOf(selectedProfile), 0, true);
            UpdateProfileValues(selectedProfile);
            UseSettingsSwitch.On = AgoraSettings.Current.UseMySettings;
        }

        partial void UseSettingsChanged(NSObject sender)
        {
            AgoraSettings.Current.UseMySettings = UseSettingsSwitch.On;
        }

        private void UpdateProfileValues(VideoProfile profile)
        {
            ProfileNameValue.Text = profile.Name;
            ResolutionValue.Text = profile.Resolution;
            MaxBitrateValue.Text = profile.Bitrate.ToString();
            MaxFrameRateValue.Text = profile.Framerate.ToString();
            AgoraSettings.Current.Profile = profile.Id;
        }
    }
}

