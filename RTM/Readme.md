Agora.io RTM samples
========================================

This repository contains samples of using Xamarin Agora.io RTM SDK Nuget packages

[Xamarin.Agora.Rtm.Android](https://www.nuget.org/packages/Xamarin.Agora.Rtm.Android/) [![NuGet Badge](https://buildstats.info/nuget/Xamarin.Agora.Rtm.Android)](https://www.nuget.org/packages/Xamarin.Agora.Rtm.Android/)
[Xamarin.Agora.Rtm.iOS](https://www.nuget.org/packages/Xamarin.Agora.Rtm.iOS/) [![NuGet Badge](https://buildstats.info/nuget/Xamarin.Agora.Rtm.iOS)](https://www.nuget.org/packages/Xamarin.Agora.Rtm.iOS/)
[Xamarin.Agora.Rtm.Mac](https://www.nuget.org/packages/Xamarin.Agora.Rtm.Mac/) [![NuGet Badge](https://buildstats.info/nuget/Xamarin.Agora.Rtm.Mac)](https://www.nuget.org/packages/Xamarin.Agora.Rtm.Mac/)


Running samples
---------------

1. Run app on any 2 devices
1. Login with account name and enter friend or channel name 
1. Click Join button


Quick Start
-----------

1. Prepare Agora App ID, you can follow [Obtaining an App ID instructions](https://docs.agora.io/en/2.1.1/product/Video/Agora%20Basics/key_web#app-id-web) to get your App ID.
2. Run [AgoraTokenServer](../AgoraTokenServer) and get server URL
3. Replace the following empty string with the corresponding App ID and URL:
    [Shared/AgoraTestConstants.cs](../Shared/AgoraTestConstants.cs)
```
		public static string AgoraAPI
		...
        public static string TokenServerBaseUrl
```
