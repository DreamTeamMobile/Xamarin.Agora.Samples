using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using DT.Samples.Agora.Shared;
using DT.Samples.Agora.Shared.Helpers;

namespace DT.Samples.Agora.OneToOne.Droid
{
    [Activity(Label = "Settings", ParentActivity = typeof(JoinActivity))]
    public class SettingsActivity : AppCompatActivity
    {
        List<VideoProfile> _profiles;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Settings);
            _profiles = VideoProfiles.Get();
            Spinner spinner = FindViewById<Spinner>(Resource.Id.profileSpinner);
            spinner.ItemSelected += profile_ItemSelected;
            var adapter = ArrayAdapter.CreateFromResource(
                this, Resource.Array.profile_title_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
            spinner.SetSelection(GetProfileIndex(AgoraSettings.Current.Profile));
            var useMySettingsSwitch = FindViewById<SwitchCompat>(Resource.Id.useMySettingsSwitch);
            useMySettingsSwitch.Checked = AgoraSettings.Current.UseMySettings;
            useMySettingsSwitch.CheckedChange += UseMySettingsSwitch_CheckedChange;
            UpdateProfileDetails();
        }

        private int GetProfileIndex(int profileId)
        {
            return _profiles.FindIndex(profile => profile.Id == profileId);
        }

        private void profile_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            AgoraSettings.Current.Profile = _profiles[e.Position].Id;
            UpdateProfileDetails();
        }

        void UseMySettingsSwitch_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            AgoraSettings.Current.UseMySettings = e.IsChecked;
        }

        private void UpdateProfileDetails()
        {
            var currentIndex = GetProfileIndex(AgoraSettings.Current.Profile);
            FindViewById<TextView>(Resource.Id.name_value).Text = _profiles[currentIndex].Name;
            FindViewById<TextView>(Resource.Id.resolution_value).Text = _profiles[currentIndex].Resolution;
            FindViewById<TextView>(Resource.Id.bitrate_value).Text = _profiles[currentIndex].Bitrate.ToString();
            FindViewById<TextView>(Resource.Id.framerate_value).Text = _profiles[currentIndex].Framerate.ToString();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId != Android.Resource.Id.Home)
                return base.OnOptionsItemSelected(item);
            Finish();
            return true;
        }
    }
}
