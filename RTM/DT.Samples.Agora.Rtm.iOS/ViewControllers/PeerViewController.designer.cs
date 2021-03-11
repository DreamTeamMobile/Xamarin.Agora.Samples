// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace DT.Samples.Agora.Rtm.iOS
{
	[Register ("PeerViewController")]
	partial class PeerViewController
	{
		[Outlet]
		UIKit.UITextField InvitationUserTextFiled { get; set; }

		[Outlet]
		UIKit.UISwitch OfflineSwitch { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UITextField PeerTextField { get; set; }

		[Action ("DoChatPressed:")]
		partial void DoChatPressed (UIKit.UIButton sender);

		[Action ("InvitationPress:")]
		partial void InvitationPress (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (InvitationUserTextFiled != null) {
				InvitationUserTextFiled.Dispose ();
				InvitationUserTextFiled = null;
			}

			if (PeerTextField != null) {
				PeerTextField.Dispose ();
				PeerTextField = null;
			}

			if (OfflineSwitch != null) {
				OfflineSwitch.Dispose ();
				OfflineSwitch = null;
			}
		}
	}
}
