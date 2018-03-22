Agora.io Full SDK Cross-Platfrom samples
========================================

This repository contains samples of using Xamarin Agora.io FULL SDK Nuget packages ([Android](https://www.nuget.org/packages/Xamarin.Agora.Full.Android/) [![NuGet Badge](https://buildstats.info/nuget/Xamarin.Agora.Full.Android)](https://www.nuget.org/packages/Xamarin.Agora.Full.Android/)| [iOS](https://www.nuget.org/packages/Xamarin.Agora.Full.iOS/) [![NuGet Badge](https://buildstats.info/nuget/Xamarin.Agora.Full.iOS)](https://www.nuget.org/packages/Xamarin.Agora.Full.iOS/)), 
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
    [DT.Samples.Agora.Shared/AgoraTestConstants.cs](Public/DT.Samples.Agora.Shared/AgoraTestConstants.cs)
    
        
        public const string AgoraAPI = "";
        
    
   * For WebAgent app
    [Web/agent/js/index.js](Web/agent/js/index.js)
    
        
        vendorKey = ""
        

   * For WebAgent app
    [Web/agent/js/index.js](Web/agent/js/index.js)
    
        
        vendorKey = ""
        

What's Inside
-------------


In this repository you can find OneToOne Communication apps for Android, iOS and Web


* **Xamarin Android and iOS** apps in one solution /Public/DT.Samples.Agora.OneToOne.sln

* **Agora WebAgent** usage example in /Web/agent

* **Agora WebRTC** usage example in /Web/webrtc
 



