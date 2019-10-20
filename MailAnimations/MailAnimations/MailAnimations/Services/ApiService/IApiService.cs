namespace MailAnimations.Services
{
    using MailAnimations.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IApiService
    {
        Task<List<Mail>> GetMails();
        Task SendMail(Mail mail);
    }
}
