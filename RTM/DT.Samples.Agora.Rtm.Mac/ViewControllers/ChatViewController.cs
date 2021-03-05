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

    public class Message
    {
        public string UserId { get; set; }
        public string Text { get; set; }

        public Message(string userId, string text)
        {
            UserId = userId;
            Text = text;
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

            _rtmDelegate = new RtmDelegate();
            _rtmDelegate.AppendMessage += AppendMsg;

            AgoraRtm.UpdateKit(_rtmDelegate);
        }

        public override void ViewWillDisappear()
        {
            base.ViewWillDisappear();
            LeaveChannel();
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

            switch (ChatType)
            {
                case ChatType.Peer:
                    SendPeer(name, text);
                    break;
                case ChatType.Group:
                    SendChannel(name, text);
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

        public void SendPeer(string peer, string msg)
        {
            var message = new AgoraRtmMessage(msg);

            AgoraRtm.RtmKit.SendMessage(message, peer, (state) =>
            {
                Console.WriteLine($"send peer msg state: ({state})");

                var current = AgoraRtm.Current;
                AppendMsg(current, msg);
            });
        }

        public void SendChannel(string channel, string msg)
        {
            var rtmChannels = AgoraRtm.RtmKit.Channels;

            if (rtmChannels[channel] is AgoraRtmChannel rtmChannel)
            {
                var message = new AgoraRtmMessage(msg);

                rtmChannel.SendMessage(message, (state) =>
                {
                    Console.Write($"send channel msg state: {state}");

                    var current = AgoraRtm.Current;
                    if (current == null)
                        return;

                    AppendMsg(current, msg);
                });
            }
        }

        public void AppendMsg(string user, string content)
        {
            var msg = new Message(user, content);
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
