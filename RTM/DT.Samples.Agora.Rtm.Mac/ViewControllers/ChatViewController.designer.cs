// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace DT.Samples.Agora.Rtm.Mac.ViewControllers
{
	[Register ("ChatViewController")]
	partial class ChatViewController
	{
		[Outlet]
		AppKit.NSTableView Table { get; set; }

		[Action ("OnBackPressed:")]
		partial void OnBackPressed (AppKit.NSButton sender);

		[Action ("OnSendMessagePressed:")]
		partial void OnSendMessagePressed (AppKit.NSTextField sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (Table != null) {
				Table.Dispose ();
				Table = null;
			}
		}
	}
}
