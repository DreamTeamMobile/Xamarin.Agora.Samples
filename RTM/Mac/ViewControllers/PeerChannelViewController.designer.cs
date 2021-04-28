// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace DT.Samples.Agora.Rtm.Mac
{
	[Register ("PeerChannelViewController")]
	partial class PeerChannelViewController
	{
		[Outlet]
		AppKit.NSTextField ChannelTextField { get; set; }

		[Outlet]
		AppKit.NSTextField PeerTextField { get; set; }

		[Action ("OnBackPressed:")]
		partial void OnBackPressed (AppKit.NSButton sender);

		[Action ("OnChannelJoinPressed:")]
		partial void OnChannelJoinPressed (AppKit.NSButton sender);

		[Action ("OnPeerChatPressed:")]
		partial void OnPeerChatPressed (AppKit.NSButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ChannelTextField != null) {
				ChannelTextField.Dispose ();
				ChannelTextField = null;
			}

			if (PeerTextField != null) {
				PeerTextField.Dispose ();
				PeerTextField = null;
			}
		}
	}
}
