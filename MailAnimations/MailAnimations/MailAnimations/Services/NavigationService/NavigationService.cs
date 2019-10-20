[assembly: Xamarin.Forms.Dependency(typeof(MailAnimations.Services.NavigationService))]
namespace MailAnimations.Services
{
    using MailAnimations.Features;
    using Plugin.SharedTransitions;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public class NavigationService : INavigationService
    {
        private INavigation FormsNavigation
        {
            get
            {
                return Application.Current.MainPage.Navigation;
            }
        }

        public Page GetInitialView()
        {
            SharedTransitionNavigationPage navigationPage = new SharedTransitionNavigationPage(new MailListView { ViewModel = new MailListViewModel() });
            NavigationPage.SetHasNavigationBar(navigationPage.CurrentPage, false);
            return navigationPage;
        }

        public Task NavigateToNewMailAsync()
        {
            NewMailView view = new NewMailView { ViewModel = new NewMailViewModel() };
            NavigationPage.SetHasNavigationBar(view, false);
            return PushAsync(view);
        }

        public Task NavigateToProfileAsync()
        {
            ProfileView view = new ProfileView { ViewModel = new ProfileViewModel() };
            NavigationPage.SetHasNavigationBar(view, false);
            return PushAsync(view);
        }

        public Task NavigateBack()
        {
            return FormsNavigation.PopAsync();
        }

        private Task PushAsync(Page page)
        {
            return PushAsync(page, FormsNavigation);
        }

        private Task PushAsync(Page page, INavigation navigation)
        {
            return Device.InvokeOnMainThreadAsync(() => navigation.PushAsync(page, true));
        }
    }
}
