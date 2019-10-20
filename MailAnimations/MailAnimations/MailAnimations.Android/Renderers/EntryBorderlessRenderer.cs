[assembly: Xamarin.Forms.ExportRenderer(typeof(MailAnimations.Controls.EntryBorderless), typeof(MailAnimations.Droid.Renderers.EntryBorderlessRenderer))]
namespace MailAnimations.Droid.Renderers
{
    using Android.Content;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.Android;

    public class EntryBorderlessRenderer : EntryRenderer
    {
        public EntryBorderlessRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.Background = null;
            }
        }
    }
}