using System;

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
                }
            }

            public void Update(CellType type, Message message)
            {
                CellType = type;
                User = message.UserId;
                Content = message.Text;
            }
        }
}