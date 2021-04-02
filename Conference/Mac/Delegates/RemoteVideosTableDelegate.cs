using System;
using AppKit;
using Xamarin.Agora.Mac;

namespace DT.Samples.Agora.Conference.iOS
{
    public class RemoteVideosTableDelegate : NSTableViewDelegate
    {
        private const string CellIdentifier = "RemoteVideoCell";

        private RemoteVideosTableDataSource _dataSource;
        private AgoraRtcEngineKit _agoraKit;

        public RemoteVideosTableDelegate(RemoteVideosTableDataSource dataSource, AgoraRtcEngineKit agoraKit)
        {
            _dataSource = dataSource;
            _agoraKit = agoraKit;
        }

        public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            var user = _dataSource.UserList[(int)row];
            var view = (RemoteVideoCell)tableView.MakeView(CellIdentifier, this);
            if(view == null)
            {
                view = new RemoteVideoCell();
            }

            AgoraRtcVideoCanvas videoCanvas = new AgoraRtcVideoCanvas
            {
                Uid = user.Uid,
                View = view.CellView,
                RenderMode = VideoRenderMode.Adaptive
            };
            _agoraKit.SetupRemoteVideo(videoCanvas);
            view.IsHandUp = user.IsHandUp;
            view.IsAudioMuted = user.IsAudioMuted;
            view.IsVideoMuted = user.IsVideoMuted;
            return view;
        }

        public override nfloat GetRowHeight(NSTableView tableView, nint row)
        {
            return 120;
        }
    }
}
