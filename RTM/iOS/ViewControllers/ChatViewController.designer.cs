// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace DT.Samples.Agora.Rtm.iOS
{
	[Register ("ChatViewController")]
	partial class ChatViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIView inputContainView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UITextField inputTextField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.NSLayoutConstraint inputViewBottom { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UITableView TableView { get; set; }

		[Action ("OnSendImageClicked:")]
		partial void OnSendImageClicked (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (inputContainView != null) {
				inputContainView.Dispose ();
				inputContainView = null;
			}

			if (inputTextField != null) {
				inputTextField.Dispose ();
				inputTextField = null;
			}

			if (inputViewBottom != null) {
				inputViewBottom.Dispose ();
				inputViewBottom = null;
			}

			if (TableView != null) {
				TableView.Dispose ();
				TableView = null;
			}
		}
	}
}
