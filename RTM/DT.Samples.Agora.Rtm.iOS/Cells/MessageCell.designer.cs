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
    [Register ("MessageCell")]
    partial class MessageCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView LeftContentBgView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LeftContentLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView LeftUserBgView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LeftUserLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView RightContentBgView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel RightContentLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView RightUserBgView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel RightUserLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (LeftContentBgView != null) {
                LeftContentBgView.Dispose ();
                LeftContentBgView = null;
            }

            if (LeftContentLabel != null) {
                LeftContentLabel.Dispose ();
                LeftContentLabel = null;
            }

            if (LeftUserBgView != null) {
                LeftUserBgView.Dispose ();
                LeftUserBgView = null;
            }

            if (LeftUserLabel != null) {
                LeftUserLabel.Dispose ();
                LeftUserLabel = null;
            }

            if (RightContentBgView != null) {
                RightContentBgView.Dispose ();
                RightContentBgView = null;
            }

            if (RightContentLabel != null) {
                RightContentLabel.Dispose ();
                RightContentLabel = null;
            }

            if (RightUserBgView != null) {
                RightUserBgView.Dispose ();
                RightUserBgView = null;
            }

            if (RightUserLabel != null) {
                RightUserLabel.Dispose ();
                RightUserLabel = null;
            }
        }
    }
}