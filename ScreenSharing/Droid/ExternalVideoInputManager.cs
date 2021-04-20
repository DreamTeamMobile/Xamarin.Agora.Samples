using System;
using Android.Content;
using Android.Graphics;
using Android.Opengl;
using Android.Util;
using Android.Views;
using DT.Xamarin.Agora.GL;
using DT.Xamarin.Agora.MediaIO;
using IO.Agora.Advancedvideo.Externvideosource;
using IO.Agora.Advancedvideo.Externvideosource.Localvideo;
using IO.Agora.Advancedvideo.Externvideosource.Screenshare;
using IO.Agora.Api.Component.Gles;
using IO.Agora.Api.Component.Gles.Core;
using Java.Lang;

namespace DT.Samples.Agora.ScreenSharing.Droid
{
    public class ExternalVideoInputManager : Java.Lang.Object, IVideoSource
    {
        private static string TAG = "ExternalVideoInputManager";

        public const int TYPE_LOCAL_VIDEO = 1;
        public const int TYPE_SCREEN_SHARE = 2;
        public const int TYPE_AR_CORE = 3;
        private static int MAX_TEXTURE_COPY = 1;

        public const string FLAG_VIDEO_PATH = "flag-local-video";
        public const string FLAG_SCREEN_WIDTH = "screen-width";
        public const string FLAG_SCREEN_HEIGHT = "screen-height";
        public const string FLAG_SCREEN_DPI = "screen-dpi";
        public const string FLAG_FRAME_RATE = "screen-frame-rate";

        private const int DEFAULT_SCREEN_WIDTH = 640;
        private const int DEFAULT_SCREEN_HEIGHT = 480;
        private const int DEFAULT_SCREEN_DPI = 3;
        private const int DEFAULT_FRAME_RATE = 15;

        private ExternalVideoInputThread _thread;
        private int _curInputType;
        private volatile IExternalVideoInput _curVideoInput;
        private volatile IExternalVideoInput _newVideoInput;

        // RTC video interface to send video
        private volatile IVideoFrameConsumer _consumer;

        private Context _context;
        private TextureTransformer _textureTransformer;

        public ExternalVideoInputManager(Context context)
        {
            this._context = context;
        }

        void Start()
        {
            _thread = new ExternalVideoInputThread(this);
            _thread.Start();
        }

        bool SetExternalVideoInput(int type, Intent intent)
        {
            // Do not reset current input if the target type is
            // the same as the current which is still running.
            if (_curInputType == type && _curVideoInput != null
                    && _curVideoInput.IsRunning)
            {
                return false;
            }

            type = TYPE_SCREEN_SHARE;
            IExternalVideoInput input;
            switch (type)
            {
                case TYPE_LOCAL_VIDEO:
                    input = new LocalVideoInput(intent.GetStringExtra(FLAG_VIDEO_PATH));
                    if (IO.Agora.Api.Component.Constant.Textureview != null)
                    {
                        IO.Agora.Api.Component.Constant.Textureview.SurfaceTextureListener = (LocalVideoInput)input;
                    }
                    break;
                case TYPE_SCREEN_SHARE:
                    int width = intent.GetIntExtra(FLAG_SCREEN_WIDTH, DEFAULT_SCREEN_WIDTH);
                    int height = intent.GetIntExtra(FLAG_SCREEN_HEIGHT, DEFAULT_SCREEN_HEIGHT);
                    int dpi = intent.GetIntExtra(FLAG_SCREEN_DPI, DEFAULT_SCREEN_DPI);
                    int fps = intent.GetIntExtra(FLAG_FRAME_RATE, DEFAULT_FRAME_RATE);
                    Log.Info(TAG, "ScreenShare:" + width + "|" + height + "|" + dpi + "|" + fps);
                    input = new ScreenShareInput(_context, width, height, dpi, fps, intent);
                    break;
                default:
                    input = null;
                    break;
            }

            SetExternalVideoInput(input);
            _curInputType = type;
            return true;
        }

        private void SetExternalVideoInput(IExternalVideoInput source)
        {
            if (_thread != null && _thread.IsAlive)
            {
                _thread.PauseThread();
            }
            _newVideoInput = source;
        }

        void Stop()
        {
            _thread.SetThreadStopped();
        }

        /**
         * This callback initializes the video source. You can enable the camera or initialize the video
         * source and then pass one of the following return values to inform the media engine whether
         * the video source is ready.
         *
         * @param consumer The IVideoFrameConsumer object which the media engine passes back. You need
         *                 to reserve this object and pass the video frame to the media engine through
         *                 this object once the video source is initialized. See the following contents
         *                 for the definition of IVideoFrameConsumer.
         * @return true: The external video source is initialized.
         * false: The external video source is not ready or fails to initialize, the media engine stops
         * and reports the error.
         * PS:
         * When initializing the video source, you need to specify a buffer type in the getBufferType
         * method and pass the video source in the specified type to the media engine.
         */
        public bool OnInitialize(IVideoFrameConsumer consumer)
        {
            _consumer = consumer;
            return true;
        }

        /**
         * The SDK triggers this callback when the underlying media engine is ready to start video streaming.
         * You should start the video source to capture the video frame. Once the frame is ready, use
         * IVideoFrameConsumer to consume the video frame.
         *
         * @return true: The external video source is enabled and the SDK calls IVideoFrameConsumer to receive
         * video frames.
         * false: The external video source is not ready or fails to enable, the media engine stops and
         * reports the error.
         */
        public bool OnStart()
        {
            return true;
        }

        /**
         * The SDK triggers this callback when the media engine stops streaming. You should then stop
         * capturing and consuming the video frame. After calling this method, the video frames are
         * discarded by the media engine.
         */
        public void OnStop()
        {

        }

        /**
         * The SDK triggers this callback when IVideoFrameConsumer is released by the media engine. You
         * can now release the video source as well as IVideoFrameConsumer.
         */
        public void OnDispose()
        {
            Log.Error(TAG, "SwitchExternalVideo-onDispose");
            _consumer = null;
        }

        public int BufferType => MediaIO.BufferType.Texture.IntValue();
        

        public int CaptureType => MediaIO.CaptureType.Screen.IntValue();

        public int ContentHint => MediaIO.ContentHint.None.IntValue();

        private class ExternalVideoInputThread : Thread
        {
            private string TAG = "ExternalVideoInputThread";
            private int DEFAULT_WAIT_TIME = 1;

            private EglCore mEglCore;
            private EGLSurface mEglSurface;
            private int mTextureId;
            private SurfaceTexture mSurfaceTexture;
            private Surface mSurface;
            private float[] mTransform = new float[16];
            private GLThreadContext mThreadContext;
            int mVideoWidth;
            int mVideoHeight;
            private volatile bool mStopped;
            private volatile bool mPaused;
            private ExternalVideoInputManager _externalVideoInputManager;

            public ExternalVideoInputThread(ExternalVideoInputManager externalVideoInputManager)
            {
                _externalVideoInputManager = externalVideoInputManager;
            }

            private void Prepare()
            {
                mEglCore = new EglCore();
                mEglSurface = mEglCore.CreateOffscreenSurface(1, 1);
                mEglCore.MakeCurrent(mEglSurface);
                mTextureId = IO.Agora.Api.Component.Gles.Core.GlUtil.CreateTextureObject(GLES11Ext.GlTextureExternalOes);
                mSurfaceTexture = new SurfaceTexture(mTextureId);
                mSurface = new Surface(mSurfaceTexture);
                mThreadContext = new GLThreadContext();
                mThreadContext.EglCore = mEglCore;
                mThreadContext.Context = mEglCore.EGLContext;
                mThreadContext.Program = new ProgramTextureOES();
                _externalVideoInputManager._textureTransformer = new TextureTransformer(MAX_TEXTURE_COPY);
                /**Customizes the video source.
                 * Call this method to add an external video source to the SDK.*/
                IO.Agora.Api.Component.Constant.Engine.SetVideoSource(_externalVideoInputManager);
            }

            private void Release()
            {
                if (IO.Agora.Api.Component.Constant.Engine == null)
                {
                    return;
                }
                /**release external video source*/
                _externalVideoInputManager._textureTransformer.Release();
                IO.Agora.Api.Component.Constant.Engine.SetVideoSource(null);
                mSurface.Release();
                mEglCore.MakeNothingCurrent();
                mEglCore.ReleaseSurface(mEglSurface);
                mSurfaceTexture.Release();
                IO.Agora.Api.Component.Gles.Core.GlUtil.DeleteTextureObject(mTextureId);
                mTextureId = 0;
                mEglCore.Release();
            }

            public override void Run()
            {
                Prepare();

                while (!mStopped)
                {
                    if (_externalVideoInputManager._curVideoInput != _externalVideoInputManager._newVideoInput)
                    {
                        Log.Info(TAG, "New video input selected");
                        // Current video input is running, but we now
                        // introducing a new video type.
                        // The new video input type may be null, referring
                        // that we are not using any video.
                        if (_externalVideoInputManager._curVideoInput != null)
                        {
                            _externalVideoInputManager._curVideoInput.OnVideoStopped(mThreadContext);
                            Log.Info(TAG, "recycle stopped input");
                        }

                        _externalVideoInputManager._curVideoInput = _externalVideoInputManager._newVideoInput;
                        if (_externalVideoInputManager._curVideoInput != null)
                        {
                            _externalVideoInputManager._curVideoInput.OnVideoInitialized(mSurface);
                            Log.Info(TAG, "initialize new input");
                        }

                        if (_externalVideoInputManager._curVideoInput == null)
                        {
                            continue;
                        }

                        Size size = _externalVideoInputManager._curVideoInput.OnGetFrameSize();
                        mVideoWidth = size.Width;
                        mVideoHeight = size.Height;
                        mSurfaceTexture.SetDefaultBufferSize(mVideoWidth, mVideoHeight);

                        if (mPaused)
                        {
                            // If current thread is in pause state, it must be paused
                            // because of switching external video sources.
                            mPaused = false;
                        }
                    }
                    else if (_externalVideoInputManager._curVideoInput != null && !_externalVideoInputManager._curVideoInput.IsRunning)
                    {
                        // Current video source has been stopped by other
                        // mechanisms (video playing has completed, etc).
                        // A callback method is invoked to do some collect
                        // or release work.
                        // Note that we also set the new video source null,
                        // meaning at meantime, we are not introducing new
                        // video types.
                        Log.Info(TAG, "current video input is not running");
                        _externalVideoInputManager._curVideoInput.OnVideoStopped(mThreadContext);
                        _externalVideoInputManager._curVideoInput = null;
                        _externalVideoInputManager._newVideoInput = null;
                    }

                    if (mPaused || _externalVideoInputManager._curVideoInput == null)
                    {
                        WaitForTime(DEFAULT_WAIT_TIME);
                        continue;
                    }

                    try
                    {
                        mSurfaceTexture.UpdateTexImage();
                        mSurfaceTexture.GetTransformMatrix(mTransform);
                    }
                    catch (Java.Lang.Exception e)
                    {
                        e.PrintStackTrace();
                    }

                    if (_externalVideoInputManager._curVideoInput != null)
                    {
                        _externalVideoInputManager._curVideoInput.OnFrameAvailable(mThreadContext, mTextureId, mTransform);
                    }

                    mEglCore.MakeCurrent(mEglSurface);
                    GLES20.GlViewport(0, 0, mVideoWidth, mVideoHeight);

                    if (_externalVideoInputManager._consumer != null)
                    {
                        Log.Error(TAG, "publish stream with ->width:" + mVideoWidth + ",height:" + mVideoHeight);
                        /**Receives the video frame in texture,and push it out
                         * @param textureId ID of the texture
                         * @param format Pixel format of the video frame
                         * @param width Width of the video frame
                         * @param height Height of the video frame
                         * @param rotation Clockwise rotating angle (0, 90, 180, and 270 degrees) of the video frame
                         * @param timestamp Timestamp of the video frame. For each video frame, you need to set a timestamp
                         * @param matrix Matrix of the texture. The float value is between 0 and 1, such as 0.1, 0.2, and so on*/
                        _externalVideoInputManager._textureTransformer.Copy(mTextureId, MediaIO.PixelFormat.TextureOes.IntValue(), mVideoWidth, mVideoHeight);
                        _externalVideoInputManager._consumer.ConsumeTextureFrame(mTextureId,
                                MediaIO.PixelFormat.TextureOes.IntValue(),
                                mVideoWidth, mVideoHeight, 0,
                                DateTime.Now.Millisecond, mTransform);
                    }

                    // The pace at which the output Surface is sampled
                    // for video frames is controlled by the waiting
                    // time returned from the external video source.
                    WaitForNextFrame();
                }

                if (_externalVideoInputManager._curVideoInput != null)
                {
                    // The manager will cause the current
                    // video source to be stopped.
                    _externalVideoInputManager._curVideoInput.OnVideoStopped(mThreadContext);
                }
                Release();
            }

            public void PauseThread()
            {
                mPaused = true;
            }

            public void SetThreadStopped()
            {
                mStopped = true;
            }

            private void WaitForNextFrame()
            {
                int wait = _externalVideoInputManager._curVideoInput != null
                        ? _externalVideoInputManager._curVideoInput.TimeToWait()
                        : DEFAULT_WAIT_TIME;
                WaitForTime(wait);
            }

            private void WaitForTime(int time)
            {
                try
                {
                    Thread.Sleep(time);
                }
                catch (InterruptedException e)
                {
                    e.PrintStackTrace();
                }
            }
        }
    }
}

