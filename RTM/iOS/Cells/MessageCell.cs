﻿using System;
using DT.Xamarin.Agora;
using Foundation;
using UIKit;

namespace DT.Samples.Agora.Rtm.iOS
{

    public enum CellType
    {
        Left = 0,
        Right = 1
    }

    public partial class MessageCell : UITableViewCell
    {

        public static readonly NSString Key = new NSString("MessageCell");
        public static readonly UINib Nib;

        protected MessageCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib()
        {
            RightUserBgView.Layer.CornerRadius = 20;
            RightContentBgView.Layer.CornerRadius = 5;

            LeftUserBgView.Layer.CornerRadius = 20;
            LeftContentBgView.Layer.CornerRadius = 5;
        }

        private CellType _cellType = CellType.Right;
        public CellType CellType
        {
            get
            {
                return _cellType;
            }
            set
            {
                var rightHidden = value == CellType.Left;

                RightUserLabel.Hidden = rightHidden;
                RightContentLabel.Hidden = rightHidden;

                LeftUserLabel.Hidden = !rightHidden;
                LeftContentLabel.Hidden = !rightHidden;

                RightUserBgView.Hidden = rightHidden;
                RightContentBgView.Hidden = rightHidden;

                LeftUserBgView.Hidden = !rightHidden;
                LeftContentBgView.Hidden = !rightHidden;

                _cellType = value;
            }
        }

        public string User
        {
            set
            {
                switch (_cellType)
                {
                    case CellType.Left:
                        LeftUserLabel.Text = value;
                        break;
                    case CellType.Right:
                        RightUserLabel.Text = value;
                        break;
                }
            }
        }

        public string Content
        {
            set
            {
                switch (_cellType)
                {
                    case CellType.Left:
                        LeftContentLabel.Text = value;
                        break;
                    case CellType.Right:
                        RightContentLabel.Text = value;
                        break;
                }
                LeftImageView.Hidden = true;
                RightImageView.Hidden = true;
            }
        }

        private UIImage _image;
        public UIImage Image
        {
            get => _image;
            set
            {
                _image = value;
                switch (CellType)
                {
                    case CellType.Left:
                        LeftImageView.Image = value;
                        LeftImageView.Hidden = false;
                        LeftContentBgView.Hidden = true;
                        break;
                    case CellType.Right:
                        RightImageView.Image = value;
                        RightImageView.Hidden = false;
                        RightContentBgView.Hidden = true;
                        break;
                }
                LeftContentLabel.Text = string.Empty;
                RightContentLabel.Text = string.Empty;
            }
        }

        public void Update(CellType type, Message message)
        {
            CellType = type;
            User = message.UserId;
            switch (message.RtmMessage.Type)
            {
                case AgoraRtmMessageType.Text:
                    Content = message.RtmMessage.Text;
                    break;
                case AgoraRtmMessageType.Image:
                    var imageMessage = message.RtmMessage as AgoraRtmImageMessage;
                    var imgData = imageMessage.Thumbnail;
                    var image = new UIImage(imgData);
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