// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace DT.Samples.Agora.Rtm.iOS
{
    [Register ("MainViewController")]
    partial class MainViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField AccountText { get; set; }


        [Action ("DoLoginPress:")]
        partial void DoLoginPress (UIKit.UIButton sender);


        [Action ("InvitationPress:")]
        partial void InvitationPress (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (AccountText != null) {
                AccountText.Dispose ();
                AccountText = null;
            }
        }
    }
}