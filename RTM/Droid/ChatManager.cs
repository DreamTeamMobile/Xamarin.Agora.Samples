using System;
using System.Collections.Generic;
using Android.Content;
using DT.Xamarin.Agora.Rtm;
using Acr.UserDialogs;
using DT.Samples.Agora.Shared;

namespace DT.Samples.Agora.Rtm.Droid
{
    public class ChatManager
    {
        public Action<string> OnAcceptInvitation;

        private SendMessageOptions _sendMsgOptions = new SendMessageOptions();
        private Context _context;
        private RtmClient _rtmClient;
        private RtmCallManager _callManager;
        private RtmMessagePool _messagePool = new RtmMessagePool();

        private List<IRtmClientListener> _listenerList = new List<IRtmClientListener>();
        private RtmClientListener _myRtmClientListener;

        public bool OfflineMessagesEnabled
        {
            get => _sendMsgOptions.EnableOfflineMessaging;
            set => _sendMsgOptions.EnableOfflineMessaging = value;
        }

        public SendMessageOptions SendMessageOptions => _sendMsgOptions;

        public ChatManager(Context context) {
            _context = context;
        }

        public void Init() 
        {
            string appID = AgoraTestConstants.AgoraAPI;

            _myRtmClientListener = new RtmClientListener();
            _myRtmClientListener.MessageReceived += OnRtmMessageReceived;
            _myRtmClientListener.StateChanged += OnStateChanged;

            try 
            {
                _rtmClient = RtmClient.CreateInstance(_context, appID, _myRtmClientListener);

#if DEBUG
                _rtmClient.SetParameters("{\"rtm.log_filter\": 65535}");
#endif
            } 
            catch (Exception e) 
            {
                throw new Exception($"NEED TO check rtm sdk init fatal error\n {e.StackTrace}");
            }

            _callManager = _rtmClient.RtmCallManager;
            _callManager.RemoteInvitationReceived += RemoteInvitationReceived;
            _callManager.RemoteInvitationFailure+= RemoteInvitationFailure;
            _callManager.LocalInvitationReceivedByPeer += LocalInvitationReceivedByPeer;
            _callManager.RemoteInvitationAccepted += RemoteInvitationAccepted;
            _callManager.RemoteInvitationRefused += RemoteInvitationRefused;
            _callManager.LocalInvitationRefused += LocalInvitationRefused;
            _callManager.LocalInvitationAccepted += LocalInvitationAccepted;
        }

        private void RemoteInvitationAccepted(object sender, RemoteInvitationAcceptedEventArgs e)
        {
            Console.WriteLine(e.P0);
            OnAcceptInvitation?.Invoke(e.P0.CallerId);
        }

        private async void LocalInvitationAccepted(object sender, LocalInvitationAcceptedEventArgs e)
        {
            await UserDialogs.Instance.AlertAsync($"User {e.P0.CalleeId} accept your invitation", null, "Go to chat");
            Console.WriteLine(e.P0);
            OnAcceptInvitation?.Invoke(e.P0.CalleeId);
        }

        private void RemoteInvitationRefused(object sender, RemoteInvitationRefusedEventArgs e)
        {
            Console.WriteLine("Remote invitation refused");
        }

        private void LocalInvitationRefused(object sender, LocalInvitationRefusedEventArgs e)
        {
            UserDialogs.Instance.Alert($"User {e.P0.CalleeId} refuse your invitation");
        }

        private void LocalInvitationReceivedByPeer(object sender, LocalInvitationReceivedByPeerEventArgs e)
        {
            Console.WriteLine("Local invitation received by peer");
        }

        private void RemoteInvitationFailure(object sender, RemoteInvitationFailureEventArgs e)
        {
            Console.WriteLine("Remote invitation failure");
        }

        private async void RemoteInvitationReceived(object sender, RemoteInvitationReceivedEventArgs e)
        {
            var result = await UserDialogs.Instance.ConfirmAsync($"User {e.P0.CallerId} sent invitation with Content: {e.P0.Content}", "New invitation", "Accept", "Refuse");

            if(result)
            {
                AcceptInvitation(e.P0);
            }
            else 
            {
                RefuseInvitation(e.P0);
            }

            Console.WriteLine(e.P0);
        }

        public void AcceptInvitation(IRemoteInvitation invitation) 
        {
            var callback = new ResultCallback();
            callback.OnSuccessAction += (obj) => 
            {
                Console.WriteLine("Invitation accept");
            };

            callback.OnFailureAction += (obj) =>
            {
                Console.WriteLine("Invitation accept error. Try again");
            };

            _callManager.AcceptRemoteInvitation(invitation, callback);
        }

        public void RefuseInvitation(IRemoteInvitation invitation)
        {
            var callback = new ResultCallback();
            callback.OnSuccessAction += (obj) =>
            {
                UserDialogs.Instance.Alert("Invitation refuse");
            };

            callback.OnFailureAction += (obj) =>
            {
                UserDialogs.Instance.Alert("Invitation refuse error. Try again");
            };

            _callManager.RefuseRemoteInvitation(invitation, callback);
        }

        public void SendInvitation(string userName)
        {
            var callback = new ResultCallback();

            callback.OnSuccessAction += (obj) => 
            {
                UserDialogs.Instance.Alert("Success sent invitation");
            };

            callback.OnFailureAction += (obj) => 
            {
                UserDialogs.Instance.Alert("Failure sent invitation");
            };

            var invitation = _callManager.CreateLocalInvitation(userName);
            invitation.Content = "Invitation content";

            _callManager.SendLocalInvitation(invitation, callback);
        }

        private void OnStateChanged(int state, int reason)
        {
            foreach (RtmClientListener listener  in _listenerList)
            {
                listener.OnConnectionStateChanged(state, reason);
            }
        }

        private void OnRtmMessageReceived(RtmMessage rtmMessage, string peerId)
        {
            if (_listenerList.Count == 0)
            {
                _messagePool.InsertOfflineMessage(rtmMessage, peerId);
            }
            else
            {
                foreach (RtmClientListener listener in _listenerList)
                {
                    listener.OnMessageReceived(rtmMessage, peerId);
                }
            }
        }

            public RtmClient GetRtmClient() {
            return _rtmClient;
        }

        public void RegisterListener(RtmClientListener listener) {
            _listenerList.Add(listener);
        }

        public void UnregisterListener(RtmClientListener listener) {
            _listenerList.Remove(listener);
        }

        public List<RtmMessage> GetAllOfflineMessages(string peerId)
        {
            return _messagePool.GetAllOfflineMessages(peerId);
        }

        public void RemoveAllOfflineMessages(string peerId)
        {
            _messagePool.RemoveAllOfflineMessages(peerId);
        }
    }

    public class LocalInvitation : Java.Lang.Object, ILocalInvitation
    {
        public LocalInvitation(string caleeId)
        {
            CalleeId = caleeId;
        }
        public string CalleeId { get; set; }
        public string ChannelId { get; set; }
        public string Content { get; set; }
        public string Response { get; }
        public int State { get; }
    }
}