using System.Threading.Tasks;
using Codeboss.Results;

namespace ChurchManager.Domain.Features.Communication.Services
{
    public interface IEmailSender
    {
        Task<OperationResult> SendEmailAsync(string toAddress, string subject, string htmlBody);
    }
}
