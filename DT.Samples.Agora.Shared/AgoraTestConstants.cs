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
                throw new NotImplementedException();
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
                return null;
            }
        }

        public const string ShareString = "Hey check out Xamarin Agora sample app at: https://github.com/DreamTeamMobile";
	}
}
