using System.Threading.Tasks;

namespace ChurchManager.Domain.Features.Communication.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toAddress, string subject, string htmlBody);
    }
}
