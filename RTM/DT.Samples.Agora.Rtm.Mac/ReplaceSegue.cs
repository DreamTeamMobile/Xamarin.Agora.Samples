using System;
using AppKit;
using Foundation;

namespace DT.Samples.Agora.Rtm.Mac
{
    [Register("ReplaceSegue")]
    public class ReplaceSegue: NSStoryboardSegue
    {
        #region Constructors
        public ReplaceSegue() { }

        public ReplaceSegue(string identifier, NSObject sourceController, NSObject destinationController) : base(identifier, sourceController, destinationController) { }

        public ReplaceSegue(IntPtr handle) : base(handle) { }

        public ReplaceSegue(NSObjectFlag x) : base(x) { }

        #endregion

        public override void Perform()
        {
            var sourceVC = SourceController as NSViewController;
            sourceVC.View.Window.ContentViewController = DestinationController as NSViewController;
        }
    }
}
