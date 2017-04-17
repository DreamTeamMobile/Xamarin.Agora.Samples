using DT.Xamarin.Agora;

namespace DT.Samples.Agora.OneToOne.iOS
{
	public static class Extensions
	{
		public static AgoraRtcVideoProfile ToAgoraRtcVideoProfile(this int profile)
		{
			switch (profile)
			{
				case 0:
					return AgoraRtcVideoProfile.VideoProfile_120P;
				case 2:
					return AgoraRtcVideoProfile.VideoProfile_120P_3;
				case 10:
					return AgoraRtcVideoProfile.VideoProfile_180P;
				case 12:
					return AgoraRtcVideoProfile.VideoProfile_180P_3;
				case 13:
					return AgoraRtcVideoProfile.VideoProfile_180P_4;
				case 20:
					return AgoraRtcVideoProfile.VideoProfile_240P;
				case 22:
					return AgoraRtcVideoProfile.VideoProfile_240P_3;
				case 24:
					return AgoraRtcVideoProfile.VideoProfile_240P_4;
				case 30:
					return AgoraRtcVideoProfile.VideoProfile_360P;
				case 32:
					return AgoraRtcVideoProfile.VideoProfile_360P_3;
				case 33:
					return AgoraRtcVideoProfile.VideoProfile_360P_4;
				case 35:
					return AgoraRtcVideoProfile.VideoProfile_360P_6;
				case 36:
					return AgoraRtcVideoProfile.VideoProfile_360P_7;
				case 37:
					return AgoraRtcVideoProfile.VideoProfile_360P_8;
				case 38:
					return AgoraRtcVideoProfile.VideoProfile_360P_9;
				case 39:
					return AgoraRtcVideoProfile.VideoProfile_360P_10;
				case 40:
					return AgoraRtcVideoProfile.VideoProfile_480P;
				case 42:
					return AgoraRtcVideoProfile.VideoProfile_480P_3;
				case 43:
					return AgoraRtcVideoProfile.VideoProfile_480P_4;
				case 45:
					return AgoraRtcVideoProfile.VideoProfile_480P_6;
				case 47:
					return AgoraRtcVideoProfile.VideoProfile_480P_8;
				case 48:
					return AgoraRtcVideoProfile.VideoProfile_480P_9;
				case 50:
					return AgoraRtcVideoProfile.VideoProfile_720P;
				case 52:
					return AgoraRtcVideoProfile.VideoProfile_720P_3;
				case 54:
					return AgoraRtcVideoProfile.VideoProfile_720P_5;
				case 55:
					return AgoraRtcVideoProfile.VideoProfile_720P_6;
				case 100:
					return AgoraRtcVideoProfile.VideoProfile_360P_11;
			}
			return AgoraRtcVideoProfile.Default;
		}
	}
}
