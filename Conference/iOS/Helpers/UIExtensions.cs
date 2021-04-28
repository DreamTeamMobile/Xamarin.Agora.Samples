using System;
using Foundation;
using UIKit;

namespace DT.Samples.Agora.Conference.iOS
{
    public static class UIExtensions
    {
        public static void SetRoundCorners(this UITextField textField)
        {
            textField.Layer.CornerRadius = 28;
            textField.Layer.BorderColor = Theme.TitleTextColor.CGColor;
            textField.Layer.BorderWidth = 2;
            textField.BorderStyle = UITextBorderStyle.None;

            textField.EditingDidBegin += (s, e) => textField.Layer.BorderColor = Theme.TintColor.CGColor;
            textField.EditingDidEnd += (s, e) => textField.Layer.BorderColor = Theme.TitleTextColor.CGColor;
        }

        public static void SetAttributedPlaceholder(this UITextField textField, string placeholder)
        {
            textField.AttributedPlaceholder = new NSAttributedString(placeholder, null, UIColor.FromRGBA(255, 255, 255, 125));
        }

        public static void SetupKeyboardHiding(this UIView view, UITextField textField)
        {
            UITapGestureRecognizer singleTapRecognizer = new UITapGestureRecognizer(() => { textField.ResignFirstResponder(); });
            singleTapRecognizer.NumberOfTouchesRequired = 1;
            singleTapRecognizer.CancelsTouchesInView = false;
            view.AddGestureRecognizer(singleTapRecognizer);
        }
    }
}
