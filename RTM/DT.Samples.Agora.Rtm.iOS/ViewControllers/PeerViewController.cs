using System;
using DT.Samples.Agora.Rtm.iOS.Delegates;
using Foundation;
using UIKit;
using DT.Xamarin.Agora;

namespace DT.Samples.Agora.Rtm.iOS
{
    public partial class PeerViewController : UIViewController
    {
        public PeerViewController(IntPtr ptr) : base(ptr)
        {

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var _callKitDelegate = new CallKitDelegate
            {
                ViewController = this
            };

            AgoraRtm.UpdateCallKit(_callKitDelegate);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            View.EndEditing(true);
        }

        partial void DoChatPressed(UIButton sender)
        {
            if (string.IsNullOrEmpty(PeerTextField.Text))
                return;

            PerformSegue("peerToChat", NSObject.FromObject(PeerTextField.Text));
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            var identifier = segue.Identifier;
            var peer = sender.ToString();

            switch (identifier)
            {
                case "peerToChat":
                    if (segue.DestinationViewController is ChatViewController chatVC)
                    {
                        chatVC.ChannelOrPeerName = peer;
                        chatVC.ChatType = ChatType.Peer;

                    }
                    break;
                default:
                    break;
            }
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            View.EndEditing(true);
        }

        partial void InvitationPress(NSObject sender)
        {
            var invitation = new AgoraRtmLocalInvitation(InvitationUserTextFiled.Text)
            {
                Content = "Chat with me!"
            };

            AgoraRtm.CallKitManager.SendLocalInvitation(invitation, (result) => 
            { 
                if(result == AgoraRtmInvitationApiCallErrorCode.Ok)
                {
                    //Success send invitation
                }
                else 
                {
                    //Failed send invitation
                }
            });
        }
    }
}