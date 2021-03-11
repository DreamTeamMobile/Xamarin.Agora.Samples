// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace DT.Samples.Agora.Rtm.Mac
{
	[Register ("MainViewController")]
	partial class MainViewController
	{
		[Outlet]
		AppKit.NSTextField AccountText { get; set; }

		[Outlet]
		AppKit.NSButton LoginButton { get; set; }

		[Outlet]
		AppKit.NSButton OfflineSwitch { get; set; }

		[Action ("DoLoginPress:")]
		partial void DoLoginPress (AppKit.NSButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (AccountText != null) {
				AccountText.Dispose ();
				AccountText = null;
			}

			if (LoginButton != null) {
				LoginButton.Dispose ();
				LoginButton = null;
			}

			if (OfflineSwitch != null) {
				OfflineSwitch.Dispose ();
				OfflineSwitch = null;
			}
		}
	}
}
