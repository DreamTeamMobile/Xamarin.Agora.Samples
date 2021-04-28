# Conference
## RTC part
Sample solution shows how you can use Agora RTC for many-to-many connection. It is upgrated [OneToOne](../OneToOne) project with ability to connect and display more than 2 users in the room.

## RTM part
This solutions shows how you can use RTM with RTC. "HandUp" button was added for signaling and RTM is used for sending and receiving signal messages.

Quick Start
-----------
- Prepare Agora App ID, you can follow [Obtaining an App ID instructions](https://docs.agora.io/en/2.1.1/product/Video/Agora%20Basics/key_web#app-id-web) to get your App ID.
- Replace the following empty string with the corresponding App ID:
    [DT.Samples.Agora.Shared/AgoraTestConstants.cs](../Shared/AgoraTestConstants.cs)
```
        public static string AgoraAPI
```
Then Use one of the following approaches:

**Constants**
- Generate Agora RTM token using [AgoraExampleProject](https://github.com/AgoraIO/Tools/tree/master/DynamicKey/AgoraDynamicKey)
- Generate temp Agora RTC token using [Console](https://console.agora.io/)
- Replace the following empty strings with the corresponding Tokens:
    [DT.Samples.Agora.Shared/AgoraTestConstants.cs](../Shared/AgoraTestConstants.cs)
```
        public static string RtmToken
		...
		public static string RtcToken
```
**Local service**
- Run [AgoraTokenServer](../AgoraTokenServer) and get server URL
- Replace the following empty string with the corresponding URL:
    [Shared/AgoraTestConstants.cs](../Shared/AgoraTestConstants.cs)
```
        public static string TokenServerBaseUrl
```
