using System;
using Foundation;
using AppKit;
using DT.Samples.Agora.Rtm.Mac.ViewControllers;
using DT.Xamarin.Agora;

namespace DT.Samples.Agora.Rtm.Mac
{
    public enum CellType
    {
        Left, Right
    }

    public partial class MessageCell : NSTableCellView
    {
        private CellType _type;
        public CellType Type
        {
            get => _type;
            set
            {
                _type = value;
                var rightHidden = value == CellType.Left;

                RightContentLabel.Hidden = rightHidden;
                RightUserLabel.Hidden = rightHidden;

                LeftContentLabel.Hidden = !rightHidden;
                LeftUserLabel.Hidden = !rightHidden;
            }
        }

        private string _user;
        public string User
        {
            get => _user;
            set
            {
                _user = value;
                switch (Type)
                {
                    case CellType.Left:
                        LeftUserLabel.StringValue = value;
                        break;
                    case CellType.Right:
                        RightUserLabel.StringValue = value;
                        break;
                }
            }
        }

        private string _content;
        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                switch (Type)
                {
                    case CellType.Left:
                        LeftContentLabel.StringValue = value;
                        break;
                    case CellType.Right:
                        RightContentLabel.StringValue = value;
                        break;
                }
                LeftImageView.Hidden = true;
                RightImageView.Hidden = true;
            }
        }

        private NSImage _image;
        public NSImage Image
        {
            get => _image;
            set
            {
                _image = value;
                switch (Type)
                {
                    case CellType.Left:
                        LeftImageView.Image = value;
                        LeftImageView.Hidden = false;
                        break;
                    case CellType.Right:
                        RightImageView.Image = value;
                        RightImageView.Hidden = false;
                        break;
                }
                LeftContentLabel.StringValue = string.Empty;
                RightContentLabel.StringValue = string.Empty;
            }
        }

        #region Constructors

        // Called when created from unmanaged code
        public MessageCell(IntPtr handle) : base(handle) { }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public MessageCell(NSCoder coder) : base(coder) { }

        #endregion

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            RightContentLabel.UsesSingleLineMode = false;
            RightContentLabel.Cell.Wraps = true;
            RightContentLabel.Cell.Scrollable = false;

            LeftContentLabel.UsesSingleLineMode = false;
            LeftContentLabel.Cell.Wraps = true;
            LeftContentLabel.Cell.Scrollable = false;
        }

        public void Update(CellType type, Message message)
        {
            Type = type;
            User = message.UserId;
            switch(message.RtmMessage.Type)
            {
                case AgoraRtmMessageType.Text:
                    Content = message.RtmMessage.Text;
                    break;
                case AgoraRtmMessageType.Image:
                    var imageMessage = message.RtmMessage as AgoraRtmImageMessage;
                    var imgData = imageMessage.Thumbnail;
                    var image = new NSImage(imgData);
                    Image = image;
                    break;
                case AgoraRtmMessageType.Raw:
                    var rawMessage = message.RtmMessage as AgoraRtmRawMessage;
                    var rawData = rawMessage.RawData;
                    Content = $"Raw[{rawData.Length}bytes] {rawMessage.Text}";
                    break;
            }
        }
    }
}
