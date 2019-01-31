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
	[Register ("SetChannelViewController")]
	partial class SetChannelViewController
	{
		[Outlet]
		UIKit.UILabel channelName { get; set; }

		[Outlet]
		UIKit.UITextField channelNameTextField { get; set; }

		[Outlet]
		UIKit.UIButton startCall { get; set; }

		[Action ("StartCallClick:")]
		partial void StartCallClick (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (channelName != null) {
				channelName.Dispose ();
				channelName = null;
			}

			if (channelNameTextField != null) {
				channelNameTextField.Dispose ();
				channelNameTextField = null;
			}

			if (startCall != null) {
				startCall.Dispose ();
				startCall = null;
			}
		}
	}
}
