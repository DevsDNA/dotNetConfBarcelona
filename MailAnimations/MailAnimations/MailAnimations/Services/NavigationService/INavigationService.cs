namespace MailAnimations.Services
{
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public interface INavigationService
    {
        Page GetInitialView();
        Task NavigateToNewMailAsync();
        Task NavigateToProfileAsync();
        Task NavigateBack();
    }
}
