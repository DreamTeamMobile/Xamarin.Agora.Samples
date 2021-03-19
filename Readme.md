Agora.io Full SDK Cross-Platfrom samples
========================================

This repository contains samples of using Xamarin Agora.io FULL SDK Nuget packages

[Xamarin.Agora.Full.Android](https://www.nuget.org/packages/Xamarin.Agora.Full.Android/) [![NuGet Badge](https://buildstats.info/nuget/Xamarin.Agora.Full.Android)](https://www.nuget.org/packages/Xamarin.Agora.Full.Android/)

[Xamarin.Agora.Full.iOS](https://www.nuget.org/packages/Xamarin.Agora.Full.iOS/) [![NuGet Badge](https://buildstats.info/nuget/Xamarin.Agora.Full.iOS)](https://www.nuget.org/packages/Xamarin.Agora.Full.iOS/)

[Xamarin.Agora.Full.Mac](https://www.nuget.org/packages/Xamarin.Agora.Full.Mac/) [![NuGet Badge](https://buildstats.info/nuget/Xamarin.Agora.Full.Mac)](https://www.nuget.org/packages/Xamarin.Agora.Full.Mac/)

[Xamarin.Agora.Full.Forms](https://www.nuget.org/packages/Xamarin.Agora.Full.Forms/) [![NuGet Badge](https://buildstats.info/nuget/Xamarin.Agora.Full.Forms)](https://www.nuget.org/packages/Xamarin.Agora.Full.Forms/)

also contains a basic sample for Agora WebRTC


Running samples
---------------

1. Run app on any 2 devices

1. Enter same room name on both devices 

1. Click Join button


Quick Start
-----------


1. Prepare Agora App ID, you can follow [Obtaining an App ID instructions](https://docs.agora.io/en/2.1.1/product/Video/Agora%20Basics/key_web#app-id-web) to get your App ID.

1. Replace the following empty string with the corresponding App ID:

* For Android/iOS/macOS app
    [DT.Samples.Agora.Shared/AgoraTestConstants.cs](DT.Samples.Agora.Shared/AgoraTestConstants.cs)
```
        public static string AgoraAPI
```
* For Forms app
    [Forms/Xamarin.Agora.Forms/DT.Samples.Agora.Cross/DT.Samples.Agora.Cross/Consts.cs](Forms/Xamarin.Agora.Forms/DT.Samples.Agora.Cross/DT.Samples.Agora.Cross/Consts.cs)
```
        public const string AgoraKey = "<AgoraKey>";
```
* For WebRTC app enter the key on UI or in the code
    [Web/webrtc/index.html](Web/webrtc/index.html)
```
        vendorKey = ...
```
1. Prepare node.js [AgoraTokenServer](AgoraTokenServer) server for generatin agora tokens. You need to enable Primary cetificate in your agora progect and replace the following empty strings with your cettificated string and App ID:
    [AgoraTokenServer/TokenServer.js](AgoraTokenServer/TokenServer.js)
```
        var appID = "<YOUR APP ID>";
        var appCertificate = "<YOUR APP CERTIFICATE>";
```
1. Run AgoraTokenServer and replace the following epmty string with server URL:
    [DT.Samples.Agora.Shared/AgoraTestConstants.cs](DT.Samples.Agora.Shared/AgoraTestConstants.cs)
```
        public static string TokernServerBaseUrl
```

What's Inside
-------------


In this repository you can find OneToOne Communication apps for Android, iOS, Mac, Forms and Web


* **Xamarin Android, iOS and MacOS** apps in one solution [/DT.Samples.Agora.OneToOne.sln](/DT.Samples.Agora.OneToOne.sln)

* **Xamarin Forms (iOS, Android, MacOS)** apps in one solution  [Forms/Xamarin.Agora.Forms/Forms.sln](Forms/Xamarin.Agora.Forms/Forms.sln)

* **Agora WebRTC** usage example in [/Web/webrtc](/Web/webrtc)
 

Screenshots
-------------

Xamarin.Forms iOS

<img src="https://raw.githubusercontent.com/DreamTeamMobile/Xamarin.Agora.Samples/master/Screenshots/Forms/AgoraXamarinFormsiOS_00.png" width="200" />
<img src="https://raw.githubusercontent.com/DreamTeamMobile/Xamarin.Agora.Samples/master/Screenshots/Forms/AgoraXamarinFormsiOS_01.png" width="200" />

Xamarin.Forms Android

<img src="https://raw.githubusercontent.com/DreamTeamMobile/Xamarin.Agora.Samples/master/Screenshots/Forms/AgoraXamarinFormsAndroid_00.png" width="200" />
<img src="https://raw.githubusercontent.com/DreamTeamMobile/Xamarin.Agora.Samples/master/Screenshots/Forms/AgoraXamarinFormsAndroid_01.png" width="200" />

Xamarin.Forms MacOS

<img src="https://raw.githubusercontent.com/DreamTeamMobile/Xamarin.Agora.Samples/master/Screenshots/Forms/AgoraXamarinFormsMacOS_00.png" width="500" />
<img src="https://raw.githubusercontent.com/DreamTeamMobile/Xamarin.Agora.Samples/master/Screenshots/Forms/AgoraXamarinFormsMacOS_01.png" width="500" />

RTM example for Android, iOS and Mac

* **Xamarin Android, iOS and MacOS** apps in one solution [RTM/DT.Samples.Agora.Rtm.sln](RTM/DT.Samples.Agora.Rtm.sln)
