using UIKit;
using DT.Xamarin.Agora;
using Foundation;
using System.Collections.Generic;
using System;
using CoreGraphics;

namespace DT.Samples.Agora.Rtm.iOS
{
    public enum ChatType
    {
        Unknow = -1,
        Peer = 0,
        Group = 1
    }

    public partial class ChatViewController : UIViewController
    {
        public string ChannelOrPeerName;

        private ChannelDelegate _channelDelegate;
        private RtmDelegate _rtmDelegate;

        public ChatViewController(IntPtr ptr) : base(ptr) 
        {
        
        }

        List<Message> list = new List<Message>();

        ChatType _type = ChatType.Unknow;

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

        private void JoinPeer()
        {
            PressedReturnToSendText(ChannelOrPeerName, $"{AgoraRtm.Current} join");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            AddKeyboradObserver();
            UpdateViews();
            TableView.DataSource = new TableSource(list);

            _rtmDelegate = new RtmDelegate();
            _rtmDelegate.AppendMessage += AppendMsg;

            AgoraRtm.UpdateKit(_rtmDelegate);

            inputTextField.ShouldReturn += ShouldReturn;
        }

        private bool ShouldReturn(UITextField textField)
        {
            if(PressedReturnToSendText(ChannelOrPeerName, textField.Text)) 
            {
                textField.Text = null;
            }
            else
            {
                View.EndEditing(true);
            }

            return true;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            LeaveChannel();
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

        public void UpdateViews()
        {
            TableView.RowHeight = UITableView.AutomaticDimension;
            TableView.EstimatedRowHeight = 55;
        }

        public void AppendMsg(string user, string content)
        {
            var msg = new Message(user, content);
            list.Add(msg);

            var end = NSIndexPath.FromRowSection(list.Count - 1, 0);

            InvokeOnMainThread(() =>
            {
                TableView.ReloadData();
                TableView.ScrollToRow(end, UITableViewScrollPosition.Bottom, true);
            });
        }

        public void ShowAlert(string message, string title = null)
        {
            var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
            alert.AddAction(UIAlertAction.Create("ОК", UIAlertActionStyle.Default, null));

            PresentViewController(alert, true, null);
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

            if ((rtmChannels[channel] is AgoraRtmChannel rtmChannel))
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

        private void KeyboardChangeFrame(NSNotification obj)
        {
            var userInfo = obj.UserInfo;
            var endKeyboardFrameValue = userInfo[UIKeyboard.FrameEndUserInfoKey] as NSValue;
            var durationValue = userInfo[UIKeyboard.AnimationDurationUserInfoKey] as NSNumber;

            var endKeyboardFrame = endKeyboardFrameValue.CGRectValue;
            var duration = durationValue.DoubleValue;

            bool isShowing = endKeyboardFrameValue.CGRectValue.GetMaxY() <= UIScreen.MainScreen.Bounds.Height;

            UIView.Animate(duration, () =>
            {

                if (isShowing)
                {
                    var offsetY = inputContainView.Frame.GetMaxY() - endKeyboardFrameValue.CGRectValue.GetMinY();
                    if (offsetY < 0)
                    {
                        return;
                    }

                    inputViewBottom.Constant = -offsetY;
                }
                else
                {
                    inputViewBottom.Constant = 0;
                }
            });
        }

        public void AddKeyboradObserver()
        {
            NSNotificationCenter.DefaultCenter.AddObserver(UIWindow.KeyboardWillChangeFrameNotification, KeyboardChangeFrame);
        }
    }
}