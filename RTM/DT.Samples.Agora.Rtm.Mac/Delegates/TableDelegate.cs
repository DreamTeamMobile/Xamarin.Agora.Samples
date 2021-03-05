using System;
using AppKit;
using DT.Samples.Agora.Rtm.Mac.DataSources;

namespace DT.Samples.Agora.Rtm.Mac.Delegates
{
    public class TableDelegate: NSTableViewDelegate
    {
        private const string CellIdentifier = "MessageCell";
        private TableSource _dataSource;

        public TableDelegate(TableSource datasource)
        {
            _dataSource = datasource;
        }

        public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            var msg = _dataSource.Messages[(int)row];
            var cell = tableView.MakeView(CellIdentifier, this) as MessageCell;
            cell.Update(msg.UserId == AgoraRtm.Current ? CellType.Right : CellType.Left, msg);
            return cell;
        }

        public override nfloat GetRowHeight(NSTableView tableView, nint row)
        {
            return 30;
        }
    }
}
