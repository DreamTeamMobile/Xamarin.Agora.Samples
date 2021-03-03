using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace DT.Samples.Agora.Rtm.iOS
{
    public class TableSource : UITableViewDataSource
    {
        private readonly List<Message> _messageList = new List<Message>();

        public TableSource(List<Message> list)
        {
            _messageList = list;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var msg = _messageList[indexPath.Row];
            var type = msg.UserId == AgoraRtm.Current ? CellType.Right : CellType.Left;
            var cell = tableView.DequeueReusableCell("MessageCell", indexPath) as MessageCell;
            cell.Update(type, msg);
            return cell;
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return _messageList.Count;
        }
    }
}