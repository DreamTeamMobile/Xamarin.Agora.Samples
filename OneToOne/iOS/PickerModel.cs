using System;
using System.Collections.Generic;
using DT.Samples.Agora.Shared;
using UIKit;

namespace DT.Samples.Agora.OneToOne.iOS
{
    public class ProfilesModel : UIPickerViewModel
    {
        private List<VideoProfile> _myItems;
        protected int selectedIndex = 0;

        public Action<VideoProfile> ItemSelected = (profile) => { };

        public VideoProfile SelectedProfile
        {
            get { return _myItems[selectedIndex]; }
        }

        public ProfilesModel(List<VideoProfile> profiles)
        {
            _myItems = profiles;
        }

        public override nint GetComponentCount(UIPickerView picker)
        {
            return 1;
        }

        public override nint GetRowsInComponent(UIPickerView picker, nint component)
        {
            return _myItems.Count;
        }

        public override string GetTitle(UIPickerView picker, nint row, nint component)
        {
            return _myItems[(int)row].Title;
        }

        public override void Selected(UIPickerView picker, nint row, nint component)
        {
            selectedIndex = (int)row;
            ItemSelected(_myItems[selectedIndex]);
        }
    }
}
