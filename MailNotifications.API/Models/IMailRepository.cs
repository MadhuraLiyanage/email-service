using MailNotifications.Models;

namespace MailNotifications.API.Models
{
    public interface IMailRepository
    {
        Task<IEnumerable<Mail>> GetAllMail();
        Task<Mail> AddlMail(Mail mail);
        Task<Mail> UpdateMailStatus(Mail mail);
    }
}
