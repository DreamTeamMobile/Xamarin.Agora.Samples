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
	[Register ("DeviceSelectionViewController")]
	partial class DeviceSelectionViewController
	{
		[Outlet]
		AppKit.NSPopUpButton microphoneSelection { get; set; }

		[Outlet]
		AppKit.NSPopUpButton speakerSelection { get; set; }

		[Action ("didClickConfirmButton:")]
		partial void didClickConfirmButton (AppKit.NSButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (microphoneSelection != null) {
				microphoneSelection.Dispose ();
				microphoneSelection = null;
			}

			if (speakerSelection != null) {
				speakerSelection.Dispose ();
				speakerSelection = null;
			}
		}
	}
}
