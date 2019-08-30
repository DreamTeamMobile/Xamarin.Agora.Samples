using System;
namespace DT.Samples.Agora.Cross
{
    public class AgoraVideoViewHolder<T>
    {
        public AgoraVideoView VideoView { get; set; }

        public T NativeView { get; set; }

        public Guid GUID { get { return VideoView.GUID; } }

        public bool IsStatic { get { return VideoView.IsStatic; } }

        public uint StreamUID
        {
            get { return VideoView.StreamUID; }
            set { VideoView.StreamUID = value; }
        }

        public AgoraVideoViewHolder(AgoraVideoView view, T nativeView)
        {
            VideoView = view;
            NativeView = nativeView;
        }
    }
}
