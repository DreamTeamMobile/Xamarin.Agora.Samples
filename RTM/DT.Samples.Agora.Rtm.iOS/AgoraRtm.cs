using Foundation;
using DT.Xamarin.Agora;
using System;
using DT.Samples.Agora.Shared;

namespace DT.Samples.Agora.Rtm.iOS
{
    public enum LoginStatus
    {
        Online = 0,
        Offline = 1
    }

    public class AgoraRtm : NSObject
    {
        public static AgoraRtmKit RtmKit = new AgoraRtmKit(AgoraTestConstants.AgoraAPI, null);
        public static AgoraRtmCallKit CallKitManager = RtmKit.RtmCallKit;


        public static string Current;
        public static LoginStatus Status = LoginStatus.Offline;


        public static void UpdateKit(AgoraRtmDelegate del) 
        {
            if (RtmKit != null)
            {
                RtmKit.AgoraRtmDelegate = del;
            }
        }

        public static void UpdateCallKit(AgoraRtmCallDelegate calDel)
        {
            if (CallKitManager != null)
            {
                CallKitManager.CallDelegate = calDel;
            }
            else
            {
                CallKitManager = RtmKit.GetRtmCallKit();
                CallKitManager.CallDelegate = calDel;
            }
        }

        public void AcceptInvitation(AgoraRtmRemoteInvitation invitation)
        {
            CallKitManager.AcceptRemoteInvitation(invitation,(result) =>
            {
                if(result == AgoraRtmInvitationApiCallErrorCode.Ok)
                {
                    Console.WriteLine("Invitation accept");
                }
                else
                {
                    Console.WriteLine("Invitation accept error. Try again");
                }
            });
        }

        public void RefuseInvitation(AgoraRtmRemoteInvitation invitation)
        {
            CallKitManager.RefuseRemoteInvitation(invitation, (result) =>
            {
                if (result == AgoraRtmInvitationApiCallErrorCode.Ok)
                {
                    Console.WriteLine("Invitation refuse");
                }
                else
                {
                    Console.WriteLine("Invitation refuse error. Try again");
                }
            });
        }

        public void SendInvitation(string userName)
        {
            var invitation = new AgoraRtmLocalInvitation(userName);
            CallKitManager.SendLocalInvitation(invitation,(result) => 
            {
                if (result == AgoraRtmInvitationApiCallErrorCode.Ok)
                {
                    Console.WriteLine("Invitation send");
                }
                else
                {
                    Console.WriteLine("Invitation send error. Try again");
                }
            });
        }
    }
}