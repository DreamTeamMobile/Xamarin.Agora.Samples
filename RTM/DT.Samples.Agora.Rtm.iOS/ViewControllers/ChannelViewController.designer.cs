// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace DT.Samples.Agora.Rtm.iOS
{
    [Register ("ChannelViewController")]
    partial class ChannelViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ChannelConnectButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField ChannelTextField { get; set; }

        [Action ("DoJoinPressed:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void DoJoinPressed (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (ChannelConnectButton != null) {
                ChannelConnectButton.Dispose ();
                ChannelConnectButton = null;
            }

            if (ChannelTextField != null) {
                ChannelTextField.Dispose ();
                ChannelTextField = null;
            }
        }
    }
}