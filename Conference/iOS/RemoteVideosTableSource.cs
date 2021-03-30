using System;
using System.Collections.Generic;
using DT.Xamarin.Agora;
using Foundation;
using UIKit;

namespace DT.Samples.Agora.Conference.iOS
{
    public class RemoteVideInfo
    {
        public uint Uid { get; set; }
        public bool IsHandUp { get; set; }
        public bool IsAudioMuted { get; set; }
        public bool IsVideoMuted { get; set; }
    }

    public class RemoteVideosTableSource: UITableViewDataSource
    {
        public List<RemoteVideInfo> UserList { get; private set; }

        private AgoraRtcEngineKit _agoraKit;

        public RemoteVideosTableSource(List<RemoteVideInfo> userList, AgoraRtcEngineKit agoraKit)
        {
            UserList = userList;
            _agoraKit = agoraKit;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var user = UserList[indexPath.Row];
            var cell = tableView.DequeueReusableCell(RemoteVideoCell.Key, indexPath) as RemoteVideoCell;
            var container = cell.CellView;
            AgoraRtcVideoCanvas videoCanvas = new AgoraRtcVideoCanvas
            {
                Uid = user.Uid,
                View = container,
                RenderMode = VideoRenderMode.Adaptive
            };
            _agoraKit.SetupRemoteVideo(videoCanvas);
            cell.IsHandUp = user.IsHandUp;
            cell.IsAudioMuted = user.IsAudioMuted;
            cell.IsVideoMuted = user.IsVideoMuted;
            return cell;
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return UserList.Count;
        }
    }
}
