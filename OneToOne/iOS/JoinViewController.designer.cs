// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace DT.Samples.Agora.OneToOne.iOS
{
	[Register ("JoinViewController")]
	partial class JoinViewController
	{
		[Outlet]
		UIKit.UILabel AgoraVersionLabel { get; set; }

		[Outlet]
		UIKit.UITextField ChannelNameEdit { get; set; }

		[Outlet]
		UIKit.UILabel ConnectingLabel { get; set; }

		[Outlet]
		UIKit.UIImageView ConnectionImage { get; set; }

		[Outlet]
		UIKit.UITextField EncryptionKeyEdit { get; set; }

		[Outlet]
		UIKit.UIButton JoinButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ChannelNameEdit != null) {
				ChannelNameEdit.Dispose ();
				ChannelNameEdit = null;
			}

			if (ConnectingLabel != null) {
				ConnectingLabel.Dispose ();
				ConnectingLabel = null;
			}

			if (AgoraVersionLabel != null) {
				AgoraVersionLabel.Dispose ();
				AgoraVersionLabel = null;
			}

			if (EncryptionKeyEdit != null) {
				EncryptionKeyEdit.Dispose ();
				EncryptionKeyEdit = null;
			}

			if (JoinButton != null) {
				JoinButton.Dispose ();
				JoinButton = null;
			}

			if (ConnectionImage != null) {
				ConnectionImage.Dispose ();
				ConnectionImage = null;
			}
		}
	}
}
