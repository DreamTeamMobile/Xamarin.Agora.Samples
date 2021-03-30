using System.Diagnostics;
using DT.Xamarin.Agora;

namespace DT.Samples.Agora.Conference.Droid
{
    public class AgoraRtcHandler : IRtcEngineEventHandler
    {
        private RoomActivity _context;

        public AgoraRtcHandler(RoomActivity activity)
        {
            _context = activity;
        }

        public override void OnJoinChannelSuccess(string p0, int p1, int p2)
        {
            _context.OnJoinChannelSuccess(p0, p1, p2);
        }

        public override void OnError(int err)
        {
            _context.OnError(err);
        }

        public override void OnFirstRemoteVideoDecoded(int p0, int p1, int p2, int p3)
        {
            Debug.WriteLine($"DidOfflineOfUid {p0}");
            _context.OnFirstRemoteVideoDecoded(p0, p1, p2, p3);
        }

        public override void OnUserJoined(int p0, int p1)
        {
            Debug.WriteLine($"OnUserJoined {p0}");
        }

        public override void OnUserOffline(int p0, int p1)
        {
            Debug.WriteLine($"DidOfflineOfUid {p0}");
            _context.OnUserOffline((uint)p0, p1);
        }

        public override void OnUserMuteVideo(int p0, bool p1)
        {
            _context.OnUserMuteVideo((uint)p0, p1);
        }

        public override void OnUserMuteAudio(int uid, bool muted)
        {
            _context.OnUserMuteAudio((uint)uid, muted);
        }

        public override void OnFirstLocalVideoFrame(int p0, int p1, int p2)
        {
            _context.OnFirstLocalVideoFrame(p0, p1, p2);
        }

        public override void OnTokenPrivilegeWillExpire(string token)
        {
            _context.OnTokenPrivilegeWillExpire(token);
        }
    }
}
