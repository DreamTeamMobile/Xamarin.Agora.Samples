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
	[Register ("VideoChatViewController")]
	partial class VideoChatViewController
	{
		[Outlet]
		AppKit.NSView controlButtons { get; set; }

		[Outlet]
		AppKit.NSButton deviceSelectionButton { get; set; }

		[Outlet]
		AppKit.NSButton handUpButton { get; set; }

		[Outlet]
		AppKit.NSButton hungUpButton { get; set; }

		[Outlet]
		AppKit.NSButton inviteButton { get; set; }

		[Outlet]
		AppKit.NSProgressIndicator LoadingIndicator { get; set; }

		[Outlet]
		AppKit.NSView localVideo { get; set; }

		[Outlet]
		AppKit.NSImageView localVideoMuteBg { get; set; }

		[Outlet]
		AppKit.NSImageView localVideoMutedIndicator { get; set; }

		[Outlet]
		AppKit.NSButton muteButton { get; set; }

		[Outlet]
		AppKit.NSTableView RemoteUsersTableView { get; set; }

		[Outlet]
		AppKit.NSButton screenShareButton { get; set; }

		[Outlet]
		AppKit.NSButton videoMuteButton { get; set; }

		[Action ("didClickDeviceSelectionButton:")]
		partial void didClickDeviceSelectionButton (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (controlButtons != null) {
				controlButtons.Dispose ();
				controlButtons = null;
			}

			if (deviceSelectionButton != null) {
				deviceSelectionButton.Dispose ();
				deviceSelectionButton = null;
			}

			if (handUpButton != null) {
				handUpButton.Dispose ();
				handUpButton = null;
			}

			if (hungUpButton != null) {
				hungUpButton.Dispose ();
				hungUpButton = null;
			}

			if (LoadingIndicator != null) {
				LoadingIndicator.Dispose ();
				LoadingIndicator = null;
			}

			if (localVideo != null) {
				localVideo.Dispose ();
				localVideo = null;
			}

			if (localVideoMuteBg != null) {
				localVideoMuteBg.Dispose ();
				localVideoMuteBg = null;
			}

			if (localVideoMutedIndicator != null) {
				localVideoMutedIndicator.Dispose ();
				localVideoMutedIndicator = null;
			}

			if (muteButton != null) {
				muteButton.Dispose ();
				muteButton = null;
			}

			if (RemoteUsersTableView != null) {
				RemoteUsersTableView.Dispose ();
				RemoteUsersTableView = null;
			}

			if (screenShareButton != null) {
				screenShareButton.Dispose ();
				screenShareButton = null;
			}

			if (videoMuteButton != null) {
				videoMuteButton.Dispose ();
				videoMuteButton = null;
			}

			if (inviteButton != null) {
				inviteButton.Dispose ();
				inviteButton = null;
			}
		}
	}
}
