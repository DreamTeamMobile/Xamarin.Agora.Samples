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
	[Register ("MessageCell")]
	partial class MessageCell
	{
		[Outlet]
		AppKit.NSTextField LeftContentLabel { get; set; }

		[Outlet]
		AppKit.NSTextField LeftUserLabel { get; set; }

		[Outlet]
		AppKit.NSTextField RightContentLabel { get; set; }

		[Outlet]
		AppKit.NSTextField RightUserLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (LeftUserLabel != null) {
				LeftUserLabel.Dispose ();
				LeftUserLabel = null;
			}

			if (LeftContentLabel != null) {
				LeftContentLabel.Dispose ();
				LeftContentLabel = null;
			}

			if (RightUserLabel != null) {
				RightUserLabel.Dispose ();
				RightUserLabel = null;
			}

			if (RightContentLabel != null) {
				RightContentLabel.Dispose ();
				RightContentLabel = null;
			}
		}
	}
}
