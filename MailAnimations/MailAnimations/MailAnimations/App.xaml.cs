namespace MailAnimations
{
    using MailAnimations.Services;
    using Xamarin.Forms;

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            InitNavigation();
        }

        private void InitNavigation()
        {
            INavigationService navigationService = DependencyService.Get<INavigationService>();
            MainPage = navigationService.GetInitialView();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
