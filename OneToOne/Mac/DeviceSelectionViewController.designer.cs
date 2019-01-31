// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace DT.Samples.Agora.OneToOne.Mac
{
	partial class DeviceSelectionViewController
	{
		[Outlet]
		AppKit.NSPopUpButton cameraSelection { get; set; }

		[Outlet]
		AppKit.NSPopUpButton microphoneSelection { get; set; }

		[Outlet]
		AppKit.NSPopUpButton speakerSelection { get; set; }

		[Action ("didClickConfirmButton:")]
		partial void didClickConfirmButton (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (cameraSelection != null) {
				cameraSelection.Dispose ();
				cameraSelection = null;
			}

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
