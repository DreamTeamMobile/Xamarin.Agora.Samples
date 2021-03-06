using System;
using System.Collections.Generic;
using AppKit;
using DT.Samples.Agora.Rtm.Mac.Delegates;
using DT.Xamarin.Agora;
using DT.Samples.Agora.Rtm.Mac.Helpers;
using DT.Samples.Agora.Rtm.Mac.DataSources;

namespace DT.Samples.Agora.Rtm.Mac.ViewControllers
{
    public enum ChatType
    {
        Unknow = -1,
        Peer = 0,
        Group = 1
    }

    public partial class Message
    {
        public string UserId { get; set; }
        public AgoraRtmMessage RtmMessage { get; set; }

        public Message(string userId, AgoraRtmMessage rtmMessage)
        {
            UserId = userId;
            RtmMessage = rtmMessage;
        }
    }

    public interface IChatVCDelegate
    {
        void chatVCWillClose(ChatViewController controller);
        void chatVCBackToRootVC(ChatViewController controller);
    }

    public partial class ChatViewController : NSViewController
    {
        private List<Message> _messages = new List<Message>();
        private RtmDelegate _rtmDelegate;
        private ChannelDelegate _channelDelegate;

        public string ChannelOrPeerName { get; set; }
        public IChatVCDelegate Delegate { get; set; }
        public AgoraRtmChannel RtmChannel { get; set; }

        public Action<NSViewController> OnGoBack { get; set; }

        private ChatType _type;
        public ChatType ChatType
        {
            set
            {
                _type = value;
                switch (_type)
                {
                    case ChatType.Peer:
                        Title = $"Chat with {ChannelOrPeerName}";
                        JoinPeer();
                        break;
                    case ChatType.Group:
                        Title = $"Channel: {ChannelOrPeerName}";
                        CreateChannel(ChannelOrPeerName);
                        break;
                }
            }
            get
            {
                return _type;
            }
        }

        public ChatViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var dataSource = new TableSource(_messages);
            Table.DataSource = dataSource;
            Table.Delegate = new TableDelegate(dataSource);
            LoadOfflineMessages();

            _rtmDelegate = new RtmDelegate();
            _rtmDelegate.AppendMessage += AppendMsg;

            AgoraRtm.UpdateKit(_rtmDelegate);
        }

        public override void ViewWillDisappear()
        {
            base.ViewWillDisappear();
            LeaveChannel();
        }

        private void LoadOfflineMessages()
        {
            if (_type == ChatType.Peer)
            {
                var messages = AgoraRtm.GetOfflineMessages(ChannelOrPeerName);
                messages.ForEach(m => AppendMsg(ChannelOrPeerName, m));
                AgoraRtm.RemoveAllOfflineMessages(ChannelOrPeerName);
            }
        }

        partial void OnBackPressed(NSButton sender)
        {
            OnGoBack?.Invoke(this);
        }

        private void JoinPeer()
        {
            PressedReturnToSendText(ChannelOrPeerName, $"{AgoraRtm.Current} join");
        }

        private bool PressedReturnToSendText(string name, string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            var message = new AgoraRtmMessage(text);
            switch (ChatType)
            {
                case ChatType.Peer:
                    SendPeer(name, message);
                    break;
                case ChatType.Group:
                    SendChannel(name, message);
                    break;
            }

            return true;
        }

        partial void OnSendMessagePressed(NSTextField sender)
        {
            if(PressedReturnToSendText(ChannelOrPeerName, sender.StringValue))
            {
                sender.StringValue = string.Empty;
            }
        }

        partial void OnSendImagePressed(NSButton sender)
        {
            var dlg = NSOpenPanel.OpenPanel;
            dlg.CanChooseFiles = true;
            dlg.CanChooseDirectories = false;
            dlg.AllowedFileTypes = new string[] { "jpg", "png", "jpeg", "bmp" };
            if(dlg.RunModal() == 1)
            {
                var url = dlg.Urls[0];
                if(url != null)
                {
                    var path = url.Path;
                    var reqId = new Random().Next();

                    AgoraRtm.RtmKit.CreateImageMessageByUploading(path, reqId, (requestId, rtmImageMessage, rtmUploadError) =>
                    {
                        InvokeOnMainThread(() =>
                        {
                            if (rtmUploadError != AgoraRtmUploadMediaErrorCode.Ok)
                            {
                                ShowAlert($"Smth went worng. Code {rtmUploadError}", "Could not send the messgae");
                                return;
                            }

                            switch (ChatType)
                            {
                                case ChatType.Peer:
                                    SendPeer(ChannelOrPeerName, rtmImageMessage);
                                    break;
                                case ChatType.Group:
                                    SendChannel(ChannelOrPeerName, rtmImageMessage);
                                    break;
                            }
                        });
                    });
                }
            }
        }

        public void SendPeer(string peer, AgoraRtmMessage message)
        {
            var options = new AgoraRtmSendMessageOptions
            {
                EnableOfflineMessaging = AgoraRtm.OneToOneMessageType == OneToOneMessageType.Offline
            };

            AgoraRtm.RtmKit.SendMessage(message, peer, options, (state) =>
            {
                Console.WriteLine($"send peer msg state: ({state})");

                var current = AgoraRtm.Current;
                AppendMsg(current, message);
            });
        }

        public void SendChannel(string channel, AgoraRtmMessage message)
        {
            var rtmChannels = AgoraRtm.RtmKit.Channels;

            if (rtmChannels[channel] is AgoraRtmChannel rtmChannel)
            {
                var options = new AgoraRtmSendMessageOptions
                {
                    EnableOfflineMessaging = AgoraRtm.OneToOneMessageType == OneToOneMessageType.Offline
                };

                rtmChannel.SendMessage(message, options, (state) =>
                {
                    Console.Write($"send channel msg state: {state}");

                    var current = AgoraRtm.Current;
                    if (current == null)
                        return;

                    AppendMsg(current, message);
                });
            }
        }

        public void AppendMsg(string user, AgoraRtmMessage message)
        {
            var msg = new Message(user, message);
            _messages.Add(msg);

            var end = _messages.Count - 1;

            InvokeOnMainThread(() =>
            {
                Table.ReloadData();
                Table.ScrollColumnToVisible(end);
            });
        }

        public void CreateChannel(string channelName)
        {
            _channelDelegate = new ChannelDelegate();
            _channelDelegate.AppendMessage += AppendMsg;
            _channelDelegate.ShowAlert += ShowAlert;

            var channel = AgoraRtm.RtmKit.CreateChannelWithId(channelName, _channelDelegate);

            if (channel == null)
                return;

            channel.JoinWithCompletion(JoinChannelBlock);
        }

        private void JoinChannelBlock(AgoraRtmJoinChannelErrorCode errorCode)
        {
            Console.WriteLine($"join channel error: {errorCode}");

            if (errorCode == AgoraRtmJoinChannelErrorCode.Ok)
            {
                PressedReturnToSendText(ChannelOrPeerName, $"{AgoraRtm.Current} join");
            }
        }

        public void LeaveChannel()
        {
            string lChannel = "";

            switch (_type)
            {
                case ChatType.Group:
                    lChannel = ChannelOrPeerName;
                    break;
                default:
                    return;
            }

            var rtmChannels = AgoraRtm.RtmKit?.Channels;

            if (!(rtmChannels[lChannel] is AgoraRtmChannel rtmChannel))
                return;

            rtmChannel.LeaveWithCompletion(LeaveChannelBlock);
        }

        private void LeaveChannelBlock(AgoraRtmLeaveChannelErrorCode errorCode)
        {
            Console.WriteLine($"leave channel error: {errorCode}");
        }

        public void ShowAlert(string message, string title = null)
        {
            Alerts.Show(message, title);
        }
    }
}
