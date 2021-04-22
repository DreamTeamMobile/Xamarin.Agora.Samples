// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace DT.Samples.Agora.Conference.iOS
{
	[Register ("LoginViewController")]
	partial class LoginViewController
	{
		[Outlet]
		UIKit.UILabel AgoraVersionLabel { get; set; }

		[Outlet]
		UIKit.UILabel ErrorLabel { get; set; }

		[Outlet]
		UIKit.UIButton LoginButton { get; set; }

		[Outlet]
		UIKit.UITextField UserNameEdit { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (AgoraVersionLabel != null) {
				AgoraVersionLabel.Dispose ();
				AgoraVersionLabel = null;
			}

			if (LoginButton != null) {
				LoginButton.Dispose ();
				LoginButton = null;
			}

			if (UserNameEdit != null) {
				UserNameEdit.Dispose ();
				UserNameEdit = null;
			}

			if (ErrorLabel != null) {
				ErrorLabel.Dispose ();
				ErrorLabel = null;
			}
		}
	}
}
