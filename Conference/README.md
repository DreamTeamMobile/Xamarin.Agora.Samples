# Conference
## RTC part
Sample solution shows how you can use Agora RTC for many-to-many connection. It is upgrated [OneToOne](../OneToOne) project with ability to connect and display more than 2 users in the room.

## RTM part
This solutions shows how you can use RTM with RTC. "HandUp" button was added for signaling and RTM is used for sending and receiving signal messages.

Quick Start
-----------

1. Run [AgoraTokenServer](../AgoraTokenServer) and get server URL
1. Replace the following empty string with the corresponding server URL:
*
    [Shared/AgoraTestConstants.cs](../Shared/AgoraTestConstants.cs)
```
        public static string TokenServerBaseUrl
```
