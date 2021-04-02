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
		UIKit.UIImageView HandUpImage { get; set; }

		[Outlet]
		UIKit.UIView MuteAudioView { get; set; }

		[Outlet]
		UIKit.UIView MuteVideoView { get; set; }

		[Outlet]
		UIKit.UIView VideoContainer { get; set; }

		[Outlet]
		UIKit.UIView View { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (HandUpImage != null) {
				HandUpImage.Dispose ();
				HandUpImage = null;
			}

			if (MuteVideoView != null) {
				MuteVideoView.Dispose ();
				MuteVideoView = null;
			}

			if (MuteAudioView != null) {
				MuteAudioView.Dispose ();
				MuteAudioView = null;
			}

			if (VideoContainer != null) {
				VideoContainer.Dispose ();
				VideoContainer = null;
			}

			if (View != null) {
				View.Dispose ();
				View = null;
			}
		}
	}
}
