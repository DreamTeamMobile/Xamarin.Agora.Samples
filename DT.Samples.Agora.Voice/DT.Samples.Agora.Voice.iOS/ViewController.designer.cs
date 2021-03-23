// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace DT.Samples.Agora.Voice.iOS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UIView ControlButtonsView { get; set; }

		[Action ("DidClickHangUpButton:")]
		partial void DidClickHangUpButton (UIKit.UIButton sender);

		[Action ("DidClickMuteButton:")]
		partial void DidClickMuteButton (UIKit.UIButton sender);

		[Action ("DidClickSwitchSpeakerButton:")]
		partial void DidClickSwitchSpeakerButton (UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ControlButtonsView != null) {
				ControlButtonsView.Dispose ();
				ControlButtonsView = null;
			}
		}
	}
}
