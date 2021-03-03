using System;
using Acr.UserDialogs;
using DT.Xamarin.Agora;
using Foundation;
using UIKit;

namespace DT.Samples.Agora.Rtm.iOS.Delegates
{
    public class CallKitDelegate : AgoraRtmCallDelegate
    {
        public UIViewController ViewController { get; set; }

        public override void LocalInvitationAccepted(AgoraRtmCallKit callKit, AgoraRtmLocalInvitation localInvitation, string response)
        {
            var config = new AlertConfig()
            {
                Message = $"User {localInvitation.CalleeId} accept your invitation",
                OkText = "Go to chat"
            };

            config.OnAction += () => 
            {
                ViewController.PerformSegue("peerToChat", NSObject.FromObject(localInvitation.CalleeId));
            };

            UserDialogs.Instance.Alert(config);
        }

        public override void LocalInvitationCanceled(AgoraRtmCallKit callKit, AgoraRtmLocalInvitation localInvitation)
        {
            Console.WriteLine("Cancel local invitation");
        }

        public override void LocalInvitationFailure(AgoraRtmCallKit callKit, AgoraRtmLocalInvitation localInvitation, AgoraRtmLocalInvitationErrorCode errorCode)
        {
            Console.WriteLine("Failure local invitation");
        }

        public override void LocalInvitationReceivedByPeer(AgoraRtmCallKit callKit, AgoraRtmLocalInvitation localInvitation)
        {
            Console.WriteLine("Recive local invitation");
        }

        public override void LocalInvitationRefused(AgoraRtmCallKit callKit, AgoraRtmLocalInvitation localInvitation, string response)
        {
            UserDialogs.Instance.Alert($"User {localInvitation.CalleeId} refuse your invitation");
        }

        public override void RemoteInvitationAccepted(AgoraRtmCallKit callKit, AgoraRtmRemoteInvitation remoteInvitation)
        {
            ViewController.PerformSegue("peerToChat", NSObject.FromObject(remoteInvitation.CallerId));
        }

        public override void RemoteInvitationCanceled(AgoraRtmCallKit callKit, AgoraRtmRemoteInvitation remoteInvitation)
        {
            Console.WriteLine("Canceled remote invitation");
        }

        public override void RemoteInvitationFailure(AgoraRtmCallKit callKit, AgoraRtmRemoteInvitation remoteInvitation, AgoraRtmRemoteInvitationErrorCode errorCode)
        {
            Console.WriteLine("Failure remote invitation");
        }

        public override void RemoteInvitationReceived(AgoraRtmCallKit callKit, AgoraRtmRemoteInvitation remoteInvitation)
        {
            var config = new ConfirmConfig()
            {
                Message = $"User {remoteInvitation.CallerId} sent invitation with Content: {remoteInvitation.Content}",
                Title = "New invitation",
                OkText = "Accept",
                CancelText = "Refuse",
            };

            config.OnAction += (result) =>
            {
                if (result)
                {
                    callKit.AcceptRemoteInvitation(remoteInvitation, null);
                }
                else
                {
                    callKit.RefuseRemoteInvitation(remoteInvitation, null);
                }
            };

            UserDialogs.Instance.Confirm(config);
        }

        public override void RemoteInvitationRefused(AgoraRtmCallKit callKit, AgoraRtmRemoteInvitation remoteInvitation)
        {
            Console.WriteLine("Refuse remote invitation");
        }
    }
}