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
	[Register ("RemoteVideoCell")]
	partial class RemoteVideoCell
	{
		[Outlet]
		AppKit.NSImageView HandUpImage { get; set; }

		[Outlet]
		AppKit.NSImageView MuteAudioIcon { get; set; }

		[Outlet]
		AppKit.NSImageView MuteVideoIcon { get; set; }

		[Outlet]
		AppKit.NSView VideoContainerView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (MuteAudioIcon != null) {
				MuteAudioIcon.Dispose ();
				MuteAudioIcon = null;
			}

			if (MuteVideoIcon != null) {
				MuteVideoIcon.Dispose ();
				MuteVideoIcon = null;
			}

			if (HandUpImage != null) {
				HandUpImage.Dispose ();
				HandUpImage = null;
			}

			if (VideoContainerView != null) {
				VideoContainerView.Dispose ();
				VideoContainerView = null;
			}
		}
	}
}
