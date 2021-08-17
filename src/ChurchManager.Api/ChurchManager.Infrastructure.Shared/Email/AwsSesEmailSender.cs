using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;
using ChurchManager.Domain.Features.Communication.Services;

namespace ChurchManager.Infrastructure.Shared.Email
{
    public class AwsSesEmailSender : IEmailSender
    {
        private readonly string _awsAccessKeyId;
        private readonly string _awsSecretAccessKey;

        public AwsSesEmailSender(string awsAccessKeyId, string awsSecretAccessKey)
        {
            _awsAccessKeyId = awsAccessKeyId;
            _awsSecretAccessKey = awsSecretAccessKey;
        }

        public async Task SendEmailAsync(string toAddress, string subject, string htmlBody)
        {
            // Change to your from email
            string senderAddress = "connect@codeboss.tech";
            // Change to your region
            using(var client = new AmazonSimpleEmailServiceV2Client(_awsAccessKeyId, _awsSecretAccessKey, RegionEndpoint.USEast1))
            {
                var sendRequest = new SendEmailRequest
                {
                    FromEmailAddress = senderAddress,
                    Destination = new Destination
                    {
                        ToAddresses = new List<string> { toAddress }
                    },
                    Content = new EmailContent
                    {
                        Simple = new Message
                        {
                            Subject = new Content {Data = subject},
                            Body = new Body
                            {
                                Html = new Content
                                {
                                    Charset = "UTF-8",
                                    Data = htmlBody
                                },
                                Text = new Content
                                {
                                    Charset = "UTF-8",
                                    Data = htmlBody
                                }
                            }
                        }
                    }
                };
                var response = await client.SendEmailAsync(sendRequest);
            }
        }
    }
}
