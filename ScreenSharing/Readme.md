Agora.io ScreenSharing samples
========================================

This repository contains samples of using Xamarin Agora.io Full SDK Nuget packages for screen sharing.

[Xamarin.Agora.Rtm.Android](https://www.nuget.org/packages/Xamarin.Agora.Rtm.Android/) [![NuGet Badge](https://buildstats.info/nuget/Xamarin.Agora.Rtm.Android)](https://www.nuget.org/packages/Xamarin.Agora.Rtm.Android/)

[Xamarin.Agora.Rtm.iOS](https://www.nuget.org/packages/Xamarin.Agora.Rtm.iOS/) [![NuGet Badge](https://buildstats.info/nuget/Xamarin.Agora.Rtm.iOS)](https://www.nuget.org/packages/Xamarin.Agora.Rtm.iOS/)

[Xamarin.Agora.Rtm.Mac](https://www.nuget.org/packages/Xamarin.Agora.Rtm.Mac/) [![NuGet Badge](https://buildstats.info/nuget/Xamarin.Agora.Rtm.Mac)](https://www.nuget.org/packages/Xamarin.Agora.Rtm.Mac/)


Running samples
---------------

1. Run app on any 2 devices
1. Enter same room name on both devices 
1. Click Join button


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
- Generate temp Agora token using [Console](https://console.agora.io/)
- Replace the following empty string with the corresponding Token:
    [DT.Samples.Agora.Shared/AgoraTestConstants.cs](../Shared/AgoraTestConstants.cs)
```
        public static string RtcToken
```
**Local service**
- Run [AgoraTokenServer](../AgoraTokenServer) and get server URL
- Replace the following empty string with the corresponding URL:
    [Shared/AgoraTestConstants.cs](../Shared/AgoraTestConstants.cs)
```
        public static string TokenServerBaseUrl
```