using System;
using DT.Samples.Agora.Shared.Helpers;
using UIKit;
using Foundation;
using DT.Xamarin.Agora;
using DT.Samples.Agora.Shared;

namespace DT.Samples.Agora.OneToOne.iOS
{
	public partial class JoinViewController : UIViewController
	{
		protected const string QualityFormat = "Current Connection - {0}";
		protected const string AgoraVersion = "powered by agora {0}";
		public AgoraRtcQualityDelegate AgoraDelegate;
		public AgoraRtcEngineKit AgoraKit;

		public override UIStatusBarStyle PreferredStatusBarStyle()
		{
			return UIStatusBarStyle.LightContent;
		}

		protected JoinViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			AgoraDelegate = new AgoraRtcQualityDelegate(this);
			AgoraKit = AgoraRtcEngineKit.SharedEngineWithAppIdAndDelegate(AgoraTestConstants.AgoraAPI, AgoraDelegate);
			ChannelNameEdit.Text = AgoraSettings.Current.RoomName;
			ChannelNameEdit.EditingDidBegin += TextField_EditingDidBegin;
			ChannelNameEdit.EditingDidEnd += TextField_EditingDidEnd;
			EncryptionKeyEdit.Text = AgoraSettings.Current.EncryptionPhrase;
			MakeTextFieldRounded(ChannelNameEdit);
			AgoraVersionLabel.Text = string.Format(AgoraVersion, AgoraRtcEngineKit.SdkVersion);
			NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIImage.FromBundle("ic_share"), UIBarButtonItemStyle.Plain, ShareButtonCliked);
			NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIImage.FromBundle("ic_settings"), UIBarButtonItemStyle.Plain, SettingsButtonCliked);
		}

		public override void ViewWillAppear(bool animated)
		{
			AgoraKit.EnableLastmileTest();
			base.ViewWillAppear(animated);
		}

		public override void ViewWillDisappear(bool animated)
		{
			AgoraKit.DisableLastmileTest();
			base.ViewWillDisappear(animated);
		}

		void TextField_EditingDidBegin(object sender, EventArgs e)
		{
			ChannelNameEdit.Layer.BorderColor = Theme.TintColor.CGColor;
		}

		void TextField_EditingDidEnd(object sender, EventArgs e)
		{
			ChannelNameEdit.Layer.BorderColor = Theme.TitleTextColor.CGColor;
		}

		protected void MakeTextFieldRounded(UITextField textField)
		{
			textField.Layer.CornerRadius = 28;
			textField.Layer.BorderColor = Theme.TitleTextColor.CGColor;
			textField.Layer.BorderWidth = 2;
		}

		public override void ViewDidDisappear(bool animated)
		{
			AgoraSettings.Current.RoomName = ChannelNameEdit.Text;
			AgoraSettings.Current.EncryptionPhrase = EncryptionKeyEdit.Text;
			base.ViewDidDisappear(animated);
		}

		void ShareButtonCliked(object sender, EventArgs e)
		{
			var activityController = new UIActivityViewController(new NSObject[] {
				UIActivity.FromObject( "Hey check out Xamarin Agora sample app at: https://drmtm.us/xamarin-agora-ios-sample" ),
			}, null);
			var topController = UIApplication.SharedApplication.KeyWindow.RootViewController;
			while (topController.PresentedViewController != null)
			{
				topController = topController.PresentedViewController;
			}
			topController.PresentViewController(activityController, true, null);
		}

		void SettingsButtonCliked(object sender, EventArgs e)
		{
			NavigationController.PushViewController(Storyboard.InstantiateViewController("SettingsViewController") as SettingsViewController, true);
		}

		public void LastmileQuality(AgoraRtcEngineKit engine, AgoraRtcQuality agoraQuality)
		{
			string quality = string.Empty;
			switch (agoraQuality)
			{
				case AgoraRtcQuality.Excellent:
					quality = "Excellent";
					ConnectionImage.Image = UIImage.FromBundle("connection_5");
					break;
				case AgoraRtcQuality.Good:
					quality = "Good";
					ConnectionImage.Image = UIImage.FromBundle("connection_4");
					break;
				case AgoraRtcQuality.Poor:
					quality = "Poor";
					ConnectionImage.Image = UIImage.FromBundle("connection_3");
					break;
				case AgoraRtcQuality.Bad:
					quality = "Bad";
					ConnectionImage.Image = UIImage.FromBundle("connection_2");
					break;
				case AgoraRtcQuality.VBad:
					quality = "Very Bad";
					ConnectionImage.Image = UIImage.FromBundle("connection_1");
					break;
				case AgoraRtcQuality.Down:
					quality = "Down";
					ConnectionImage.Image = UIImage.FromBundle("connection_0");
					break;
				default:
					quality = "Unknown";
					ConnectionImage.Image = UIImage.FromBundle("connection_0");
					break;
			}
			ConnectingLabel.Text = string.Format(QualityFormat, quality);
		}
	}
}

