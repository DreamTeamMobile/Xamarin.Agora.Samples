using AppKit;

namespace DT.Samples.Agora.Rtm.Mac.Helpers
{
    public class Alerts
    {
        public static void Show(string message, string title)
        {
            var alert = new NSAlert()
            {
                AlertStyle = NSAlertStyle.Informational,
                InformativeText = message,
                MessageText = title
            };
            alert.RunModal();
        }
    }
}
