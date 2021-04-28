// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace DT.Samples.Agora.Voice.Mac
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSView ControlButtonsView { get; set; }

		[Outlet]
		AppKit.NSButton EndCallButton { get; set; }

		[Outlet]
		AppKit.NSTextField ErrorLabel { get; set; }

		[Outlet]
		AppKit.NSButton JoinButton { get; set; }

		[Outlet]
		AppKit.NSButton MuteButton { get; set; }

		[Action ("DidClickDeviceButton:")]
		partial void DidClickDeviceButton (AppKit.NSButton sender);

		[Action ("DidClickHangUpButton:")]
		partial void DidClickHangUpButton (AppKit.NSButton sender);

		[Action ("DidClickMuteButton:")]
		partial void DidClickMuteButton (AppKit.NSButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ControlButtonsView != null) {
				ControlButtonsView.Dispose ();
				ControlButtonsView = null;
			}

			if (EndCallButton != null) {
				EndCallButton.Dispose ();
				EndCallButton = null;
			}

			if (JoinButton != null) {
				JoinButton.Dispose ();
				JoinButton = null;
			}

			if (MuteButton != null) {
				MuteButton.Dispose ();
				MuteButton = null;
			}

			if (ErrorLabel != null) {
				ErrorLabel.Dispose ();
				ErrorLabel = null;
			}
		}
	}
}
