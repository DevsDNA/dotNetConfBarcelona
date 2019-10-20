[assembly: Xamarin.Forms.ExportEffect(typeof(MailAnimations.iOS.Effects.ScrollEffect), "ScrollEffect")]
namespace MailAnimations.iOS.Effects
{
    using Foundation;
    using MailAnimations.Common;
    using System;
    using System.Linq;
    using UIKit;
    using Xamarin.Forms.Platform.iOS;

    public class ScrollEffect : PlatformEffect
    {
        private IDisposable offsetObserver;
        private MailAnimations.Effects.ScrollEffect effect;

        public ScrollEffect()
        {
        }

        protected override void OnAttached()
        {
            offsetObserver?.Dispose();
            effect = (MailAnimations.Effects.ScrollEffect)Element.Effects.FirstOrDefault(e => e is MailAnimations.Effects.ScrollEffect);
            offsetObserver = Control.AddObserver("contentOffset", NSKeyValueObservingOptions.New, HandleAction);
        }

        protected override void OnDetached()
        {
            offsetObserver?.Dispose();
            offsetObserver = null;
        }


        private void HandleAction(NSObservedChange obj)
        {
            double effectiveY = ((IUIFocusItemScrollableContainer)Control).ContentOffset.Y;
            effect.RaiseScrollChanged(effectiveY);
        }
    }
}