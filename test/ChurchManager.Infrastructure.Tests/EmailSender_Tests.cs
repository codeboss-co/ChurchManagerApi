using System.Threading.Tasks;
using ChurchManager.Infrastructure.Shared.Email;
using Xunit;

namespace ChurchManager.Infrastructure.Tests
{
    public class EmailSender_Tests
    {
        [Fact]
        public async Task Test_sending_emails()
        {
            var sender = new AwsSesEmailSender("AKIA6HLAUN2QAA2ZQ5W3", "PMd1zeIjRc2Nja0qwa+RzU3y5/xdqolEDODGQoqX");

            var operationResult = await sender.SendEmailAsync("dillancagnetta@yahoo.com", "EmailSender Tests", "Hello there");

            Assert.True(operationResult.IsSuccess);
        }
    }
}
