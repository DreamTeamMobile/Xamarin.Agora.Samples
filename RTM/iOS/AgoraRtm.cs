using Foundation;
using DT.Xamarin.Agora;
using System;
using DT.Samples.Agora.Shared;
using System.Collections.Generic;

namespace DT.Samples.Agora.Rtm.iOS
{
    public enum LoginStatus
    {
        Online = 0,
        Offline = 1
    }

    public enum OneToOneMessageType
    {
        Normal,
        Offline
    }

    public class AgoraRtm : NSObject
    {
        public static AgoraRtmKit RtmKit = new AgoraRtmKit(AgoraTestConstants.AgoraAPI, null);
        public static AgoraRtmCallKit CallKitManager = RtmKit.RtmCallKit;


        public static string Current { get; set; }
        public static LoginStatus Status { get; set; } = LoginStatus.Offline;
        public static OneToOneMessageType OneToOneMessageType { get; set; } = OneToOneMessageType.Normal;

        private static Dictionary<string, List<AgoraRtmMessage>> _offlineMessages = new Dictionary<string, List<AgoraRtmMessage>>();

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

        public static void AddOfflineMessage(AgoraRtmMessage message, string user)
        {
            if (!message.IsOfflineMessage)
                return;

            var messages = new List<AgoraRtmMessage>();
            if(_offlineMessages.ContainsKey(user))
            {
                messages = _offlineMessages[user];
            }
            messages.Add(message);
            _offlineMessages[user] = messages;
        }

        public static List<AgoraRtmMessage> GetOfflineMessages(string user)
        {
            return _offlineMessages.ContainsKey(user) ? _offlineMessages[user] : new List<AgoraRtmMessage>();
        }

        public static void RemoveAllOfflineMessages(string user)
        {
            if (_offlineMessages.ContainsKey(user))
                _offlineMessages.Remove(user);
        }
    }
}