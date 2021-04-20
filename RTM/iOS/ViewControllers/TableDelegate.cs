using System;
using Foundation;
using UIKit;

namespace DT.Samples.Agora.Rtm.iOS.ViewControllers
{
    public class TableDelegate : UITableViewDelegate
    {
        private TableSource _dataSource;

        public TableDelegate(TableSource datasource)
        {
            _dataSource = datasource;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            var msg = _dataSource.Messages[indexPath.Row];
            switch (msg.RtmMessage.Type)
            {
                case Xamarin.Agora.AgoraRtmMessageType.Text:
                    return 50;
                case Xamarin.Agora.AgoraRtmMessageType.Image:
                    return 100;
                default:
                    return 50;
            }
        }
    }
}
