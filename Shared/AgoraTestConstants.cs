using System;
namespace DT.Samples.Agora.Shared
{
	public static class AgoraTestConstants
	{
        /// <summary>
        /// App ID from https://dashboard.agora.io/
        /// </summary>
		public static string AgoraAPI
        {
            get
            {
                return "67c8032f7ad544cca83a431a0b0e4cf3";
            }
        }

        public static string TokenServerBaseUrl
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Temp token generated in https://dashboard.agora.io/ or Token from your API
        /// </summary>
        public static string Token
        {
            get
            {
                return "00667c8032f7ad544cca83a431a0b0e4cf3IACoNpGwKpGYkuk5jSuwWQigVv66krxRkB0cTf/D516SG6ADl5IAAAAAEADGTqVpC5GAYAEAAQALkYBg";
            }
        }

        public const string ShareString = "Hey check out Xamarin Agora sample app at: https://github.com/DreamTeamMobile";
	}
}
