using UIKit;
using DT.Xamarin.Agora;
using Foundation;
using System.Collections.Generic;
using System;
using CoreGraphics;
using DT.Samples.Agora.Rtm.iOS.ViewControllers;

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

        List<Message> _list = new List<Message>();

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

            var tableSource = new TableSource(_list);
            TableView.DataSource = tableSource;
            TableView.Delegate = new TableDelegate(tableSource);
            LoadOfflineMessages();

            _rtmDelegate = new RtmDelegate();
            _rtmDelegate.AppendMessage += AppendMsg;

            AgoraRtm.UpdateKit(_rtmDelegate);

            inputTextField.ShouldReturn += ShouldReturn;
        }

        private bool ShouldReturn(UITextField textField)
        {
            if (PressedReturnToSendText(ChannelOrPeerName, textField.Text))
            {
                textField.Text = null;
            }
            else
            {
                View.EndEditing(true);
            }

            return true;
        }

        partial void OnSendImageClicked(UIButton sender)
        {
            var imagePicker = new UIImagePickerController
            {
                SourceType = UIImagePickerControllerSourceType.PhotoLibrary,
                MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary)
            };
            imagePicker.FinishedPickingMedia += (s, e) =>
            {
                imagePicker.DismissModalViewController(true);
                var path = e.ImageUrl.AbsoluteString;

                try
                {
                    new AgoraMediaUploader().UploadMediaByPath(AgoraRtm.RtmKit, path, 1, (reqId, rtmImageMessage, rtmUploadError) =>
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
                }
                catch(Exception ex)
                {

                }
            };
            imagePicker.Canceled += (s, e) => imagePicker.DismissModalViewController(true);
            PresentViewController(imagePicker, true, null);
        }

        private void OnFinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
        {
            
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

        public void UpdateViews()
        {
            TableView.RowHeight = UITableView.AutomaticDimension;
            TableView.EstimatedRowHeight = 55;
        }

        private void LoadOfflineMessages()
        {
            if(_type == ChatType.Peer)
            {
                var messages = AgoraRtm.GetOfflineMessages(ChannelOrPeerName);
                messages.ForEach(m => AppendMsg(ChannelOrPeerName, m));
                AgoraRtm.RemoveAllOfflineMessages(ChannelOrPeerName);
            }
        }

        public void AppendMsg(string user, AgoraRtmMessage message)
        {
            var msg = new Message(user, message);
            _list.Add(msg);

            var end = NSIndexPath.FromRowSection(_list.Count - 1, 0);

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

                rtmChannel.SendMessage(message, options,(state) =>
                {
                    Console.Write($"send channel msg state: {state}");

                    var current = AgoraRtm.Current;
                    if (current == null)
                        return;

                    AppendMsg(current, message);
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