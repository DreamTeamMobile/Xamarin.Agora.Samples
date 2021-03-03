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
    [Register ("PeerViewController")]
    partial class PeerViewController
    {
        [Outlet]
        UIKit.UITextField InvitationUserTextFiled { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField PeerTextField { get; set; }


        [Action ("DoChatPressed:")]
        partial void DoChatPressed (UIKit.UIButton sender);


        [Action ("InvitationPress:")]
        partial void InvitationPress (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (InvitationUserTextFiled != null) {
                InvitationUserTextFiled.Dispose ();
                InvitationUserTextFiled = null;
            }

            if (PeerTextField != null) {
                PeerTextField.Dispose ();
                PeerTextField = null;
            }
        }
    }
}