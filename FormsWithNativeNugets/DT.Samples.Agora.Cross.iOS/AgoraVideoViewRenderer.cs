using System;
using System.ComponentModel;
using System.Drawing;
using Foundation;
using UIKit;
using DT.Samples.Agora.Cross.iOS;
using Xamarin.Forms;
using DT.Samples.Agora.Cross;

[assembly: ExportRenderer(typeof(AgoraVideoView), typeof(AgoraVideoViewRenderer))]
namespace DT.Samples.Agora.Cross.iOS
{
    /// <summary>
    /// Agora video view renderer.
    /// </summary>
    public class AgoraVideoViewRenderer : global::Xamarin.Forms.Platform.iOS.ViewRenderer
    {
        private UIView _layout;
        private AgoraVideoView _callView;
        private AgoraVideoViewHolder<UIView> _holder;
        private AgoraServiceImplementation _videoService;

        /// <summary>
        /// On the element changed.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnElementChanged(global::Xamarin.Forms.Platform.iOS.ElementChangedEventArgs<global::Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null)
                return;
            _callView = e.NewElement as AgoraVideoView;
            _layout = new UIView(new RectangleF(0, 0, (float)Element.Width, (float)Element.Height)) { Hidden = false };
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
                        _layout = new UIView(new RectangleF(0, 0, (float)Element.Width, (float)Element.Height)) { Hidden = false };
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
        /// Gets the size of the desired.
        /// </summary>
        /// <returns>The desired size.</returns>
        /// <param name="widthConstraint">Width constraint.</param>
        /// <param name="heightConstraint">Height constraint.</param>
        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            return new SizeRequest(global::Xamarin.Forms.Size.Zero, global::Xamarin.Forms.Size.Zero);
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

        protected AgoraVideoViewHolder<UIView> UpdatedHolder()
        {
            if(_holder == null)
            {
                _holder = new AgoraVideoViewHolder<UIView>(_callView, _layout);
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
