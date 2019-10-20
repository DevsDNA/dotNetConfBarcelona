using ReactiveUI;
namespace MailAnimations.Base
{
    using ReactiveUI.XamForms;
    using System;
    using System.Reactive.Linq;

    public class BaseViewCell<TViewModel> : ReactiveViewCell<TViewModel> where TViewModel : class
    {
        public BaseViewCell()
        {
            this.WhenAnyValue(x => x.ViewModel)
                .Where(x => x != null)
                .Do(PopulateInfo)
                .Subscribe();
        }

        protected virtual void PopulateInfo(TViewModel viewModel)
        {
            
        }
    }
}
