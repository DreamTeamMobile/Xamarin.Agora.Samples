using System;
using System.Collections.Generic;
using AppKit;
using DT.Samples.Agora.Rtm.Mac.ViewControllers;

namespace DT.Samples.Agora.Rtm.Mac.DataSources
{
    public class TableSource: NSTableViewDataSource
    {
        public List<Message> Messages = new List<Message>();

        public TableSource(List<Message> messages)
        {
            Messages = messages;
        }

        public override nint GetRowCount(NSTableView tableView)
        {
            return Messages.Count;
        }
    }
}
