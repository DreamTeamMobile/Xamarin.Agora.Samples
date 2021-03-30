using System;
using System.Collections.Generic;
using AppKit;

namespace DT.Samples.Agora.Conference.iOS
{
    public class RemoteVideoInfo
    {
        public uint Uid { get; set; }
        public bool IsHandUp { get; set; }
        public bool IsAudioMuted { get; set; }
        public bool IsVideoMuted { get; set; }
    }

    public class RemoteVideosTableDataSource : NSTableViewDataSource
    {
        public List<RemoteVideoInfo> UserList { get; private set; }

        public RemoteVideosTableDataSource(List<RemoteVideoInfo> userList)
        {
            UserList = userList;
        }

        public override nint GetRowCount(NSTableView tableView)
        {
            return UserList.Count;
        }
    }
}
