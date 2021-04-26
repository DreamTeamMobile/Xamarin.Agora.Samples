using System;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Widget;

namespace DT.Samples.Agora.Conference.Droid
{
    public class BaseActivity : AppCompatActivity
    {
        protected void ShowAlertDialog(string title, string okButton = "OK", Action okClicked = null, Action cancelCLicked = null)
        {
            var dialogBuilder = new AlertDialog.Builder(this, Resource.Style.DT_Theme_AlertDialog);
            dialogBuilder.SetTitle(title);
            dialogBuilder.SetPositiveButton(okButton, (senderAlert, args) =>
            {
                okClicked?.Invoke();
            });
            dialogBuilder.SetNegativeButton("Cancel", (senderAlert, args) =>
            {
                cancelCLicked?.Invoke();
            });
            dialogBuilder.Show();
        }

        protected void ShowEntryAlertDialog(string title, string hint, string okButton = "OK", Action<string> okClicked = null, Action cancelCLicked = null)
        {
            var dialogBuilder = new AlertDialog.Builder(this, Resource.Style.DT_Theme_AlertDialog);
            dialogBuilder.SetTitle(title);
            var editText = new EditText(this)
            {
                Hint = hint
            };
            editText.SetTextColor(new Color(ContextCompat.GetColor(BaseContext, Resource.Color.textColorDialog)));
            dialogBuilder.SetView(editText);
            dialogBuilder.SetPositiveButton(okButton, (senderAlert, args) =>
            {
                okClicked?.Invoke(editText.Text);
            });
            dialogBuilder.SetNegativeButton("Cancel", (senderAlert, args) =>
            {
                cancelCLicked?.Invoke();
            });
            dialogBuilder.Show();
        }
    }
}
