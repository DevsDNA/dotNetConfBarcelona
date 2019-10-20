[assembly: Xamarin.Forms.ExportRenderer(typeof(MailAnimations.Controls.EntryBorderless), typeof(MailAnimations.iOS.Renderers.EntryBorderlessRenderer))]
namespace MailAnimations.iOS.Renderers
{
    using System.ComponentModel;
    using UIKit;
    using Xamarin.Forms.Platform.iOS;

    public class EntryBorderlessRenderer : EntryRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control != null)
            {
                Control.Layer.BorderWidth = 0;
                Control.BorderStyle = UITextBorderStyle.None;
            }
        }
    }
}