// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AgoraTutorial.iOS
{
	[Register ("VideoCallViewController")]
	partial class VideoCallViewController
	{
		[Outlet]
		UIKit.UIView controlButtons { get; set; }

		[Outlet]
		UIKit.UIButton hangUpButton { get; set; }

		[Outlet]
		UIKit.UIView localVideo { get; set; }

		[Outlet]
		UIKit.UIImageView localVideoMutedBg { get; set; }

		[Outlet]
		UIKit.UIImageView localVideoMutedIndicator { get; set; }

		[Outlet]
		UIKit.UIButton muteButton { get; set; }

		[Outlet]
		UIKit.UIView remoteVideo { get; set; }

		[Outlet]
		UIKit.UIImageView remoteVideoMutedIndicator { get; set; }

		[Outlet]
		UIKit.UIButton switchCameraButton { get; set; }

		[Outlet]
		UIKit.UIButton videoMuteButton { get; set; }

		[Action ("HangUpButtonClick:")]
		partial void HangUpButtonClick (Foundation.NSObject sender);

		[Action ("MuteButtonClick:")]
		partial void MuteButtonClick (Foundation.NSObject sender);

		[Action ("SwitchCameraButtonClick:")]
		partial void SwitchCameraButtonClick (Foundation.NSObject sender);

		[Action ("VideoMuteButtonClick:")]
		partial void VideoMuteButtonClick (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (controlButtons != null) {
				controlButtons.Dispose ();
				controlButtons = null;
			}

			if (hangUpButton != null) {
				hangUpButton.Dispose ();
				hangUpButton = null;
			}

			if (localVideo != null) {
				localVideo.Dispose ();
				localVideo = null;
			}

			if (localVideoMutedBg != null) {
				localVideoMutedBg.Dispose ();
				localVideoMutedBg = null;
			}

			if (localVideoMutedIndicator != null) {
				localVideoMutedIndicator.Dispose ();
				localVideoMutedIndicator = null;
			}

			if (muteButton != null) {
				muteButton.Dispose ();
				muteButton = null;
			}

			if (remoteVideo != null) {
				remoteVideo.Dispose ();
				remoteVideo = null;
			}

			if (remoteVideoMutedIndicator != null) {
				remoteVideoMutedIndicator.Dispose ();
				remoteVideoMutedIndicator = null;
			}

			if (switchCameraButton != null) {
				switchCameraButton.Dispose ();
				switchCameraButton = null;
			}

			if (videoMuteButton != null) {
				videoMuteButton.Dispose ();
				videoMuteButton = null;
			}
		}
	}
}
