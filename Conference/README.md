# Conference
## RTC part
Sample solution shows how you can use Agora RTC for many-to-many connection. It is upgrated [OneToOne](../OneToOne) project with ability to connect and display more than 2 users in the room.

## RTM part
This solutions shows how you can use RTM with RTC. "HandUp" button was added for signaling and RTM is used for sending and receiving signal messages.

## Quick start
1. Prepare Agora App ID, you can follow [Obtaining an App ID instructions](https://docs.agora.io/en/2.1.1/product/Video/Agora%20Basics/key_web#app-id-web) to get your App ID.
2. Run [AgoraTokenServer](../AgoraTokenServer) for getting agora tokens.
3. Replace the following empty strings with the corresponding values:
[DT.Samples.Agora.Shared/AgoraTestConstants.cs](DT.Samples.Agora.Shared/AgoraTestConstants.cs)
```
public static string AgoraAPI
public static string TokenServerBaseUrl
```