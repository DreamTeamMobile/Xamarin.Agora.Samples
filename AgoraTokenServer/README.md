# How to use
## Fill in your vendor information
Open *TokenServer.js* and replace <YOUR APP ID> and <YOUR APP CERTIFICATE> with your value
```
// Fill the appID and appCertificate key given by Agora.io
var appID = "<YOUR APP ID>";
var appCertificate = "<YOUR APP CERTIFICATE>";
```

## Install Dependencies and Run

```shell
npm i
node TokenServer.js
```

### Expose local web server

You can use [ngrok](https://ngrok.com/) for exposing local web server to the internet.
The following command will share your local web server
```
ngrok http 8080
```

## Generate Token
### Generate RTC Token
```shell
curl [server_url]/rtcToken?channelName=test
```

### Generate RTM Token
```shell
curl [server_url]/rtmToken?account=testAccount
```
