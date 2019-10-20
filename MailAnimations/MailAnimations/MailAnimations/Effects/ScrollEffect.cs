namespace MailAnimations.Effects
{
    using System;
    using Xamarin.Forms;

    public class ScrollEffect : RoutingEffect
    {
        public ScrollEffect() : base("MailAnimations.ScrollEffect")
        {
        }

        public event EventHandler<double> ScrollChanged;

        public void RaiseScrollChanged(double scroll)
        {
            ScrollChanged?.Invoke(this, scroll);
        }
    }
}
