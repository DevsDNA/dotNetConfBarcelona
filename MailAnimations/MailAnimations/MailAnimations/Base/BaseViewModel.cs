namespace MailAnimations.Base
{
    using MailAnimations.Services;
    using ReactiveUI;
    using System.Reactive.Disposables;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public class BaseViewModel : ReactiveObject
    {
        protected readonly INavigationService navigationService;

        public BaseViewModel()
        {
            navigationService = DependencyService.Get<INavigationService>();

            Disposables = new CompositeDisposable();
        }

        public CompositeDisposable Disposables { get; set; }

        public virtual Task AppearingAsync()
        {
            Disposables = Disposables ?? new CompositeDisposable();
            return Task.CompletedTask;
        }

        public virtual Task DisappearingAsync()
        {
            Disposables?.Dispose();
            Disposables = null;
            return Task.CompletedTask;
        }
    }
}
