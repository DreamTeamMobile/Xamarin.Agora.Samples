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
	[Register ("RoomViewController")]
	partial class RoomViewController
	{
		[Outlet]
		UIKit.UITapGestureRecognizer BackgroundDoubleTap { get; set; }

		[Outlet]
		UIKit.UITapGestureRecognizer BackgroundTap { get; set; }

		[Outlet]
		UIKit.UIView ContainerView { get; set; }

		[Outlet]
		UIKit.UILabel DebugData { get; set; }

		[Outlet]
		UIKit.UIButton EndCallButton { get; set; }

		[Outlet]
		UIKit.UIButton HandUpButton { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView LoadingIndicator { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint LocalVideoHeight { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint LocalVideoWidth { get; set; }

		[Outlet]
		UIKit.UIView LocalView { get; set; }

		[Outlet]
		UIKit.UIView MutedView { get; set; }

		[Outlet]
		UIKit.UITableView RemoteVideosContainer { get; set; }

		[Outlet]
		UIKit.UIStackView RemoteVideosStack { get; set; }

		[Outlet]
		UIKit.UILabel RoomNameLabel { get; set; }

		[Outlet]
		UIKit.UIButton SwitchCamButton { get; set; }

		[Outlet]
		UIKit.UIButton ToggleAudioButton { get; set; }

		[Outlet]
		UIKit.UIButton ToggleCamButton { get; set; }

		[Action ("DoBackDoubleTapped:")]
		partial void DoBackDoubleTapped (Foundation.NSObject sender);

		[Action ("DoBackTapped:")]
		partial void DoBackTapped (Foundation.NSObject sender);

		[Action ("EndCallClicked:")]
		partial void EndCallClicked (Foundation.NSObject sender);

		[Action ("InviteUserButtonClicked:")]
		partial void InviteUserButtonClicked (UIKit.UIButton sender);

		[Action ("OnHandUpButtonClicked:")]
		partial void OnHandUpButtonClicked (UIKit.UIButton sender);

		[Action ("SwitchCamClicked:")]
		partial void SwitchCamClicked (Foundation.NSObject sender);

		[Action ("ToggleAudioButtonClicked:")]
		partial void ToggleAudioButtonClicked (Foundation.NSObject sender);

		[Action ("ToggleCamClicked:")]
		partial void ToggleCamClicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (BackgroundDoubleTap != null) {
				BackgroundDoubleTap.Dispose ();
				BackgroundDoubleTap = null;
			}

			if (BackgroundTap != null) {
				BackgroundTap.Dispose ();
				BackgroundTap = null;
			}

			if (ContainerView != null) {
				ContainerView.Dispose ();
				ContainerView = null;
			}

			if (DebugData != null) {
				DebugData.Dispose ();
				DebugData = null;
			}

			if (EndCallButton != null) {
				EndCallButton.Dispose ();
				EndCallButton = null;
			}

			if (HandUpButton != null) {
				HandUpButton.Dispose ();
				HandUpButton = null;
			}

			if (LoadingIndicator != null) {
				LoadingIndicator.Dispose ();
				LoadingIndicator = null;
			}

			if (LocalVideoHeight != null) {
				LocalVideoHeight.Dispose ();
				LocalVideoHeight = null;
			}

			if (LocalVideoWidth != null) {
				LocalVideoWidth.Dispose ();
				LocalVideoWidth = null;
			}

			if (LocalView != null) {
				LocalView.Dispose ();
				LocalView = null;
			}

			if (MutedView != null) {
				MutedView.Dispose ();
				MutedView = null;
			}

			if (RemoteVideosContainer != null) {
				RemoteVideosContainer.Dispose ();
				RemoteVideosContainer = null;
			}

			if (RemoteVideosStack != null) {
				RemoteVideosStack.Dispose ();
				RemoteVideosStack = null;
			}

			if (RoomNameLabel != null) {
				RoomNameLabel.Dispose ();
				RoomNameLabel = null;
			}

			if (SwitchCamButton != null) {
				SwitchCamButton.Dispose ();
				SwitchCamButton = null;
			}

			if (ToggleAudioButton != null) {
				ToggleAudioButton.Dispose ();
				ToggleAudioButton = null;
			}

			if (ToggleCamButton != null) {
				ToggleCamButton.Dispose ();
				ToggleCamButton = null;
			}
		}
	}
}
