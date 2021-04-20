using System;
using Foundation;
using AppKit;
using DT.Samples.Agora.Rtm.Mac.ViewControllers;

namespace DT.Samples.Agora.Rtm.Mac
{
	public partial class PeerChannelViewController : NSViewController
	{
		private ChatType _selectedChatType = ChatType.Unknow;
		public Action<NSViewController> OnGoBack { get; set; }

		public PeerChannelViewController (IntPtr handle) : base (handle)
		{
		}

		partial void OnChannelJoinPressed(NSButton sender)
        {
			var channel = ChannelTextField.StringValue;
			if (string.IsNullOrEmpty(channel))
				return;

			_selectedChatType = ChatType.Group;
			PerformSegue("peerChannelToChat", this);
		}

		partial void OnPeerChatPressed(NSButton sender)
		{
			var peer = PeerTextField.StringValue;
			if (string.IsNullOrEmpty(peer))
				return;

			_selectedChatType = ChatType.Peer;
			PerformSegue("peerChannelToChat", this);
		}

		partial void OnBackPressed(NSButton sender)
		{
			OnGoBack?.Invoke(this);
		}

		public override void PrepareForSegue(NSStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
			if (segue.DestinationController is ChatViewController chatVC)
			{
				chatVC.OnGoBack += (vc) => vc.View.Window.ContentViewController = this;
				switch (_selectedChatType)
				{
					case ChatType.Group:
						chatVC.ChannelOrPeerName = ChannelTextField.StringValue;
						chatVC.ChatType = ChatType.Group;
						break;
					case ChatType.Peer:
						chatVC.ChannelOrPeerName = PeerTextField.StringValue;
						chatVC.ChatType = ChatType.Peer;
						break;
				}
			}
		}
    }
}
