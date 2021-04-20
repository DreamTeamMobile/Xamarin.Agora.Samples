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
	[Register ("MainViewController")]
	partial class MainViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UITextField AccountText { get; set; }

		[Outlet]
		UIKit.UISwitch OfflineSwitch { get; set; }

		[Action ("DoLoginPress:")]
		partial void DoLoginPress (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (AccountText != null) {
				AccountText.Dispose ();
				AccountText = null;
			}

			if (OfflineSwitch != null) {
				OfflineSwitch.Dispose ();
				OfflineSwitch = null;
			}
		}
	}
}
