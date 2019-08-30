using System;
using System.ComponentModel;
using AppKit;
using DT.Samples.Agora.Cross;
using DT.Samples.Agora.Cross.MacOS;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(AgoraVideoView), typeof(AgoraVideoViewRenderer))]
namespace DT.Samples.Agora.Cross.MacOS
{
    /// <summary>
    /// Agora video view renderer.
    /// </summary>
    public class AgoraVideoViewRenderer : global::Xamarin.Forms.Platform.MacOS.ViewRenderer
    {
        private NSView _layout;
        private AgoraVideoView _callView;
        private AgoraVideoViewHolder<NSView> _holder;
        private AgoraServiceImplementation _videoService;

        /// <summary>
        /// On the element changed.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnElementChanged(global::Xamarin.Forms.Platform.MacOS.ElementChangedEventArgs<global::Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null)
                return;
            _callView = e.NewElement as AgoraVideoView;
            var width = e.NewElement.Width;
            var height = e.NewElement.Height;
            _layout = new NSView(new CoreGraphics.CGRect(0, 0, (nfloat)Element.Width, (nfloat)Element.Height)); //(new RectangleF(0, 0, width, height)) { Hidden = false };
            _videoService = DependencyService.Get<IAgoraService>() as AgoraServiceImplementation;
            SetNativeControl(_layout);
            _videoService.SetupView(UpdatedHolder());
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AgoraVideoView.StreamUID) || e.PropertyName == nameof(AgoraVideoView.Mode))
            {
                try
                {
                    if (_callView.IsStatic)
                    {
                        _layout = new NSView(new CoreGraphics.CGRect(0, 0, (nfloat)Element.Width, (nfloat)Element.Height)); //(new RectangleF(0, 0, width, height)) { Hidden = false };
                        _videoService = DependencyService.Get<IAgoraService>() as AgoraServiceImplementation;
                        SetNativeControl(_layout);
                        _videoService.SetupView(UpdatedHolder());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    _holder.NativeView?.RemoveFromSuperview();
                }
            }
            base.OnElementPropertyChanged(sender, e);
        }

        /// <summary>
        /// Fits the size of the to new.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public void FitToNewSize(double width, double height)
        {
            _layout.Frame = Frame;
        }

        protected AgoraVideoViewHolder<NSView> UpdatedHolder()
        {
            if (_holder == null)
            {
                _holder = new AgoraVideoViewHolder<NSView>(_callView, _layout);
            }
            else
            {
                _holder.NativeView?.RemoveFromSuperview();
                _holder.NativeView = _layout;
                _holder.VideoView = _callView;
            }
            return _holder;
        }
    }
}
