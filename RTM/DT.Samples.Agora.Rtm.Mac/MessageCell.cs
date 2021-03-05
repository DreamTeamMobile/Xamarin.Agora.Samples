using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;
using DT.Samples.Agora.Rtm.Mac.ViewControllers;

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
            Content = message.Text;
        }
    }
}
