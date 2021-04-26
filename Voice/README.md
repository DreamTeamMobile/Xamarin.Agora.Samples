Agora.io Voice samples
========================================

This repository contains samples of using Xamarin Agora.io Voice SDK Nuget packages.

[Xamarin.Agora.Voice.Android](https://www.nuget.org/packages/Xamarin.Agora.Voice.Android/) [![NuGet Badge](https://buildstats.info/nuget/Xamarin.Agora.Voice.Android)](https://www.nuget.org/packages/Xamarin.Agora.Voice.Android/)
[Xamarin.Agora.Voice.iOS](https://www.nuget.org/packages/Xamarin.Agora.Voice.iOS/) [![NuGet Badge](https://buildstats.info/nuget/Xamarin.Agora.Voice.iOS)](https://www.nuget.org/packages/Xamarin.Agora.Voice.iOS/)
[Xamarin.Agora.Voice.Mac](https://www.nuget.org/packages/Xamarin.Agora.Voice.Mac/) [![NuGet Badge](https://buildstats.info/nuget/Xamarin.Agora.Voice.Mac)](https://www.nuget.org/packages/Xamarin.Agora.Voice.Mac/)


Running samples
---------------

Run app on any 2 devices. By default you will be connected to `drmtm.us` channel


Quick Start
-----------

1. Prepare Agora App ID, you can follow [Obtaining an App ID instructions](https://docs.agora.io/en/2.1.1/product/Video/Agora%20Basics/key_web#app-id-web) to get your App ID.
1. Generate temp Agora token using [Console](https://console.agora.io/)
1. Replace the following empty string with the corresponding App ID and Token:
    [DT.Samples.Agora.Shared/AgoraTestConstants.cs](../Shared/AgoraTestConstants.cs)
```
        public static string AgoraAPI
        ...
        public static string Token
```