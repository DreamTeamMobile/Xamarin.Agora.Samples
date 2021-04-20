// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace DT.Samples.Agora.ScreenSharing.iOS
{
    [Register("SettingsViewController")]
    partial class SettingsViewController
    {
        [Outlet]
        UIKit.UILabel MaxBitrateValue { get; set; }

        [Outlet]
        UIKit.UILabel MaxFrameRateValue { get; set; }

        [Outlet]
        UIKit.UILabel ProfileNameValue { get; set; }

        [Outlet]
        UIKit.UIPickerView ProfilePicker { get; set; }

        [Outlet]
        UIKit.UILabel ResolutionValue { get; set; }

        [Outlet]
        UIKit.UISwitch UseSettingsSwitch { get; set; }

        [Action("UseSettingsChanged:")]
        partial void UseSettingsChanged(NSObject sender);

        void ReleaseDesignerOutlets()
        {
            if (ProfileNameValue != null)
            {
                ProfileNameValue.Dispose();
                ProfileNameValue = null;
            }

            if (ResolutionValue != null)
            {
                ResolutionValue.Dispose();
                ResolutionValue = null;
            }

            if (MaxBitrateValue != null)
            {
                MaxBitrateValue.Dispose();
                MaxBitrateValue = null;
            }

            if (MaxFrameRateValue != null)
            {
                MaxFrameRateValue.Dispose();
                MaxFrameRateValue = null;
            }

            if (ProfilePicker != null)
            {
                ProfilePicker.Dispose();
                ProfilePicker = null;
            }

            if (UseSettingsSwitch != null)
            {
                UseSettingsSwitch.Dispose();
                UseSettingsSwitch = null;
            }
        }
    }
}
