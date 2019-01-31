Agora.io Full SDK Cross-Platfrom samples
========================================

This repository contains samples of using Xamarin Agora.io FULL SDK Nuget packages

[Xamarin.Agora.Full.Android](https://www.nuget.org/packages/Xamarin.Agora.Full.Android/) [![NuGet Badge](https://buildstats.info/nuget/Xamarin.Agora.Full.Android)](https://www.nuget.org/packages/Xamarin.Agora.Full.Android/)

[Xamarin.Agora.Full.iOS](https://www.nuget.org/packages/Xamarin.Agora.Full.iOS/) [![NuGet Badge](https://buildstats.info/nuget/Xamarin.Agora.Full.iOS)](https://www.nuget.org/packages/Xamarin.Agora.Full.iOS/)

[Xamarin.Agora.Full.Mac](https://www.nuget.org/packages/Xamarin.Agora.Full.Mac/) [![NuGet Badge](https://img.shields.io/nuget/v/Xamarin.Agora.Full.Mac.svg)](https://www.nuget.org/packages/Xamarin.Agora.Full.Mac/)

[Xamarin.Agora.Full.Forms](https://www.nuget.org/packages/Xamarin.Agora.Full.Forms/) [![NuGet Badge](https://img.shields.io/nuget/v/Xamarin.Agora.Full.Forms.svg)](https://www.nuget.org/packages/Xamarin.Agora.Full.Forms/)

also contains simple samples for Agora WebRTC and WebAgent SDK


Running samples
-------------

1. Run app on any 2 devices

1. Enter same room name on both devices 

1. Click Join button


Quick Start
-----------


1. Prepare Agora App ID, you can follow [Obtaining an App ID instructions](https://docs.agora.io/en/2.1.1/product/Video/Agora%20Basics/key_web#app-id-web) to get your App ID.

1. Replace the following empty string with the corresponding App ID:

* For Android/iOS app
    [DT.Samples.Agora.Shared/AgoraTestConstants.cs](DT.Samples.Agora.Shared/AgoraTestConstants.cs)
```
        public const string AgoraAPI = "";
```
* For Forms app
    [Forms/Xamarin.Agora.Forms/DT.Samples.Agora.Cross/DT.Samples.Agora.Cross/Consts.cs](Forms/Xamarin.Agora.Forms/DT.Samples.Agora.Cross/DT.Samples.Agora.Cross/Consts.cs)
```
        public const string AgoraKey = "<AgoraKey>";
```
* For Mac app
    [Mac/Xamarin.Agora.Mac/DT.Samples.Agora.Mac/VideoChatViewController.cs](Mac/Xamarin.Agora.Mac/DT.Samples.Agora.Mac/VideoChatViewController.cs)
```
         protected const string AgoraKey = "<AgoraKey>";
```
* For WebRTC app
    [Web/webrtc/index.html](Web/webrtc/index.html)
```
        vendorKey = ""
```
* For WebAgent app
    [Web/agent/js/index.js](Web/agent/js/index.js)
```
        vendorKey = ""
```

What's Inside
-------------


In this repository you can find OneToOne Communication apps for Android, iOS, Mac, Forms and Web


* **Xamarin Android and iOS** apps in one solution [/DT.Samples.Agora.OneToOne.sln](/DT.Samples.Agora.OneToOne.sln)

* **Xamarin Forms (iOS, Android, Mac)** apps in one solution  [Forms/Xamarin.Agora.Forms/Forms.sln](Forms/Xamarin.Agora.Forms/Forms.sln)

* **Xamarin Mac** apps in one solution [Mac/Xamarin.Agora.Mac/DT.Samples.Agora.Mac.sln](Mac/Xamarin.Agora.Mac/DT.Samples.Agora.Mac.sln)

* **Agora WebAgent** usage example in [/Web/agent](/Web/agent)

* **Agora WebRTC** usage example in [/Web/webrtc](/Web/webrtc)
 



