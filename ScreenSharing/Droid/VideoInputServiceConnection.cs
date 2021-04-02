using System;
using Android.App;
using Android.Content;
using Android.Media.Projection;
using Android.OS;
using IO.Agora.Advancedvideo.Externvideosource;

namespace DT.Samples.Agora.ScreenSharing.Droid
{
    public class VideoInputServiceConnection: Java.Lang.Object, IServiceConnection
    {
        public static int PROJECTION_REQ_CODE = 10;

        private Activity _context;
        public Action<IExternalVideoInputService> ServiceConnected;

        public VideoInputServiceConnection(Activity context)
        {
            _context = context;
        }

        public void OnServiceConnected(ComponentName name, IBinder binder)
        {
            var service = (IExternalVideoInputService)binder;
            ServiceConnected?.Invoke(service);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                var mpm = (MediaProjectionManager)
                        _context.GetSystemService(Context.MediaProjectionService);
                var intent = mpm.CreateScreenCaptureIntent();
                _context.StartActivityForResult(intent, PROJECTION_REQ_CODE);
            }
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            throw new NotImplementedException();
        }
    }
}
