using System.Collections.Generic;

namespace DT.Samples.Agora.Shared
{
    public class VideoProfile
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public int Id { get; set; }
        public int Bitrate { get; set; }
        public int Framerate { get; set; }
        public string Resolution { get; set; }
    }

    public static class VideoProfiles
    {
        public static List<VideoProfile> Get()
        {
            return new List<VideoProfile> {
                new VideoProfile { Title ="160x120 15fps", Name ="120P", Bitrate =65, Framerate =15, Id =0, Resolution ="160x120" },
                new VideoProfile { Title ="120x120 15fps", Name ="120P_3", Bitrate =50, Framerate =15, Id =2, Resolution ="120x120" },
                new VideoProfile { Title ="320x180 15fps", Name ="180P", Bitrate =140, Framerate =15, Id =10, Resolution ="320x180" },
                new VideoProfile { Title ="180x180 15fps", Name ="180P_3", Bitrate =100, Framerate =15, Id =12, Resolution ="180x180" },
                new VideoProfile { Title ="240x180 15fps", Name ="180P_4", Bitrate =120, Framerate =15, Id =13, Resolution ="240x180" },
                new VideoProfile { Title ="320x240 15fps", Name ="240P", Bitrate =200, Framerate =15, Id =20, Resolution ="320x240" },
                new VideoProfile { Title ="240x240 15fps", Name ="240P_3", Bitrate =140, Framerate =15, Id =22, Resolution ="240x240" },
                new VideoProfile { Title ="424x240 15fps", Name ="240P_4", Bitrate =220, Framerate =15, Id =24, Resolution ="424x240" },
                new VideoProfile { Title ="640x360 15fps", Name ="360P", Bitrate =400, Framerate =15, Id =30, Resolution ="640x360" },
                new VideoProfile { Title ="360x360 15fps", Name ="360P_3", Bitrate =260, Framerate =15, Id =32, Resolution ="360x360" },
                new VideoProfile { Title ="640x360 30fps", Name ="360P_4", Bitrate =600, Framerate =30, Id =33, Resolution ="640x360" },
                new VideoProfile { Title ="360x360 30fps", Name ="360P_6", Bitrate =400, Framerate =30, Id =35, Resolution ="360x360" },
                new VideoProfile { Title ="480x360 15fps", Name ="360P_7", Bitrate =320, Framerate =15, Id =36, Resolution ="480x360" },
                new VideoProfile { Title ="480x360 30fps", Name ="360P_8", Bitrate =490, Framerate =30, Id =37, Resolution ="480x360" },
                new VideoProfile { Title ="640x360 15fps", Name ="360P_9", Bitrate =800, Framerate =15, Id =38, Resolution ="640x360" },
                new VideoProfile { Title ="640x360 24fps", Name ="360P_10", Bitrate =800, Framerate =24, Id =39, Resolution ="640x360" },
                new VideoProfile { Title ="640x360 24fps", Name ="360P_11", Bitrate =1000, Framerate =24, Id =100, Resolution ="640x360" },
                new VideoProfile { Title ="640x480 15fps", Name ="480P", Bitrate =500, Framerate =15, Id =40, Resolution ="640x480" },
                new VideoProfile { Title ="480x480 15fps", Name ="480P_3", Bitrate =400, Framerate =15, Id =42, Resolution ="480x480" },
                new VideoProfile { Title ="640x480 30fps", Name ="480P_4", Bitrate =750, Framerate =30, Id =43, Resolution ="640x480" },
                new VideoProfile { Title ="480x480 30fps", Name ="480P_6", Bitrate =600, Framerate =30, Id =45, Resolution ="480x480" },
                new VideoProfile { Title ="848x480 15fps", Name ="480P_8", Bitrate =610, Framerate =15, Id =47, Resolution ="848x480" },
                new VideoProfile { Title ="848x480 30fps", Name ="480P_9", Bitrate =930, Framerate =30, Id =48, Resolution ="848x480" },
                new VideoProfile { Title ="1280x720 15fps", Name ="720P", Bitrate =1130, Framerate =15, Id =50, Resolution ="1280x720" },
                new VideoProfile { Title ="1280x720 30fps", Name ="720P_3", Bitrate =1710, Framerate =30, Id =52, Resolution ="1280x720" },
                new VideoProfile { Title ="960x720 15fps", Name ="720P_5", Bitrate =910, Framerate =15, Id =54, Resolution ="960x720" },
                new VideoProfile { Title ="960x720 30fps", Name ="720P_6", Bitrate =1380, Framerate =30, Id =55, Resolution ="960x720" }
            };
        }
    }
}
