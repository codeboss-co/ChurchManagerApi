using System.Threading.Tasks;
using ChurchManager.Domain.Features.People;
using Codeboss.Results;

namespace ChurchManager.Domain.Features.Communication.Services
{
    public interface IEmailSender
    {
        Task<OperationResult> SendEmailAsync(string toAddress, string subject, string htmlBody);
        Task<OperationResult> SendEmailAsync(Email email, string subject, string htmlBody);
        Task<OperationResult> SendEmailAsync(Person to, string subject, string htmlBody);
    }
}
