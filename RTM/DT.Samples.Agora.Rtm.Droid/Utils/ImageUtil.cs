using System;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Com.Bumptech.Glide;
using DT.Xamarin.Agora.Rtm;
using Java.IO;
using Java.Lang;
using Java.Util.Concurrent;

namespace DT.Samples.Agora.Rtm.Droid.Utils
{
    public class ImageUtil
    {
        private const string LogTag = "ImageUtil";
        private static string CacheDir = "rtm_image_disk_cache";

        public static string GetCacheFile(Context context, string id)
        {
            var parent = new File(context.CacheDir, CacheDir);
            if(!parent.Exists())
            {
                parent.Mkdirs();
            }
            return new File(parent, id).AbsolutePath;
        }

        public static byte[] BitmapToByteArray(Bitmap bitmap)
        {
            var stream = new System.IO.MemoryStream();
            bitmap.Compress(Bitmap.CompressFormat.Jpeg, 10, stream);
            return stream.ToArray();
        }

        public static byte[] PreloadImage(Context context, string file, int width, int height)
        {
            try
            {
                var options = new BitmapFactory.Options();
                options.InPreferredConfig = Bitmap.Config.Argb8888;
                var bitmap2 = BitmapFactory.DecodeFile(file, options);

                var bitmap = Glide.With(context)
                    .AsBitmap()
                    .Load(file)
                    .Submit(width, height)
                    .Get() as Bitmap;
                return BitmapToByteArray(bitmap2);
            }
            catch(ExecutionException e)
            {
                e.PrintStackTrace();
            }
            catch(InterruptedException e)
            {
                e.PrintStackTrace();
            }
            return null;
        }

        public static void UploadImage(Context context, RtmClient rtmClient, string file, ResultCallback resultCallback)
        {
            var createResultCallback = new ResultCallback();
            createResultCallback.OnSuccessAction += (data) =>
            {
                Log.Debug(LogTag, $"UploadImage File: ${file}");
                var rtmImageMessage = data as RtmImageMessage;
                Log.Debug(LogTag, $"UploadImage MediaId: ${rtmImageMessage.MediaId}");
                var width = rtmImageMessage.Width / 5;
                var height = rtmImageMessage.Height / 5;
                rtmImageMessage.SetThumbnail(PreloadImage(context, file, width, height));
                rtmImageMessage.ThumbnailWidth = width;
                rtmImageMessage.ThumbnailHeight = height;

                resultCallback.OnSuccess(rtmImageMessage);
            };
            createResultCallback.OnFailureAction += (err) => resultCallback.OnFailureAction(err);
            rtmClient.CreateImageMessageByUploading(file, new RtmRequestId(), createResultCallback);
        }

        public static void CacheImage(Context context, RtmClient rtmClient, RtmImageMessage rtmImageMessage, ResultCallback resultCallback)
        {
            var cacheFile = GetCacheFile(context, rtmImageMessage.MediaId);
            if(new File(cacheFile).Exists())
            {
                resultCallback.OnSuccess(cacheFile);
            }
            else
            {
                var downloadResultCallback = new ResultCallback();
                downloadResultCallback.OnSuccessAction += (obj) => resultCallback.OnSuccess(cacheFile);
                downloadResultCallback.OnFailureAction += (err) => resultCallback.OnFailureAction(err);
                rtmClient.DownloadMediaToFile(rtmImageMessage.MediaId,
                    cacheFile,
                    new RtmRequestId(),
                    downloadResultCallback);
            }
        }
    }
}
