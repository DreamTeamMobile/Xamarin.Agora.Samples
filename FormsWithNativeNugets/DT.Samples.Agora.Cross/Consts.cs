using System;
namespace DT.Samples.Agora.Cross
{
    public static class Consts
    {
        public static string AgoraKey { get { throw new NotImplementedException(); } } // return "[App Id]"
        public static string Token { get { return null; } }
        /// <summary>
        /// The unknown local stream identifier.
        /// </summary>
        public const uint UnknownLocalStreamId = 0;

        /// <summary>
        /// The unknown remote stream identifier.
        /// </summary>
        public const uint UnknownRemoteStreamId = 1;
    }
}
