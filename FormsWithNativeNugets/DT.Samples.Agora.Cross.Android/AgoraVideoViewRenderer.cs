using System;
using System.ComponentModel;
using Android.Runtime;
using Android.Widget;
using DT.Samples.Agora.Cross;
using DT.Samples.Agora.Cross.Droid;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(AgoraVideoView), typeof(AgoraVideoViewRenderer))]
namespace DT.Samples.Agora.Cross.Droid
{
    /// <summary>
    /// Agora video view renderer.
    /// </summary>
    public class AgoraVideoViewRenderer : global::Xamarin.Forms.Platform.Android.ViewRenderer
    {
        private AgoraServiceImplementation _videoService;
        private FrameLayout _layout;
        private AgoraVideoView _callView;
        private AgoraVideoViewHolder<FrameLayout> _holder;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Xamarin.Agora.Full.Forms.AgoraVideoViewRenderer"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        public AgoraVideoViewRenderer(global::Android.Content.Context context) : base(context) { }
        /// <summary>
        /// Fits the size of the to new.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public void FitToNewSize(double width, double height)
        {
        }
        /// <summary>
        /// On the element changed.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnElementChanged(global::Xamarin.Forms.Platform.Android.ElementChangedEventArgs<global::Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null)
                return;
            global::Android.App.Application.SynchronizationContext.Post(_ =>
            {
                _callView = e.NewElement as AgoraVideoView;
                _layout = new FrameLayout(Context)
                {
                    LayoutParameters = new FrameLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent)
                };
                _videoService = DependencyService.Get<IAgoraService>() as AgoraServiceImplementation;
                SetNativeControl(_layout);
                _videoService.SetupView(UpdatedHolder());
            }, null);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AgoraVideoView.StreamUID) || e.PropertyName == nameof(AgoraVideoView.Mode))
            {
                global::Android.App.Application.SynchronizationContext.Post(_ =>
                {
                    try
                    {
                        if (_callView.IsStatic)
                        {
                            _layout = new FrameLayout(Context)
                            {
                                LayoutParameters = new FrameLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent)
                            };
                            _videoService = DependencyService.Get<IAgoraService>() as AgoraServiceImplementation;

                            SetNativeControl(_layout);
                            _videoService.SetupView(UpdatedHolder());
                        }
                    }
                    catch(Exception ex)
                    {
                        _holder.NativeView?.RemoveAllViews();
                    }
                }, null);
            }
            base.OnElementPropertyChanged(sender, e);
        }

        protected AgoraVideoViewHolder<FrameLayout> UpdatedHolder()
        {
            if (_holder == null)
            {
                _holder = new AgoraVideoViewHolder<FrameLayout>(_callView, _layout);
            }
            else
            {
                _holder.NativeView?.RemoveAllViews();
                _holder.NativeView = _layout;
                _holder.VideoView = _callView;
            }
            return _holder;
        }
    }
}

