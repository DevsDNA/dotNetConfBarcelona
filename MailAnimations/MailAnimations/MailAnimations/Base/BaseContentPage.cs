namespace MailAnimations.Base
{
    using ReactiveUI;
    using ReactiveUI.XamForms;
    using System.Reactive.Disposables;
    using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

    public class BaseContentPage<TViewModel> : ReactiveContentPage<TViewModel> where TViewModel : BaseViewModel
    {
        public BaseContentPage()
        {
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            this.WhenActivated(d => ManageDisposables(d));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel?.AppearingAsync();
        }

        protected override void OnDisappearing()
        {
            ViewModel?.DisappearingAsync();
            base.OnDisappearing();
        }

        protected virtual void CreateBindings(CompositeDisposable disposables)
        { }

        protected virtual void ManageEvents(CompositeDisposable disposables)
        { }

        private CompositeDisposable ManageDisposables(CompositeDisposable disposables)
        {
            CreateBindings(disposables);
            ManageEvents(disposables);

            return disposables;
        }
    }
}
