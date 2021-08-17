using System.IO;
using System.Threading.Tasks;
using ChurchManager.Infrastructure.Shared.Email;
using ChurchManager.Infrastructure.Shared.Templating;
using ChurchManager.Infrastructure.Tests.Templates;
using Newtonsoft.Json;
using Xunit;

namespace ChurchManager.Infrastructure.Tests
{
    public class EmailSender_Tests
    {
        private string _awsAccessKeyId = "AKIA6HLAUN2QAA2ZQ5W3";
        private string _awsSecretAccessKey = "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";
        private string _toEmailAddress = "dillancagnetta@yahoo.com";

        private string _templateFilePath = "./Templates/ExampleTemplate.liquid";
        private string _templateExampleData = "./Templates/ExampleData.json";

        [Fact]
        public async Task Test_sending_emails()
        {
            var sender = new AwsSesEmailSender(_awsAccessKeyId, _awsSecretAccessKey);

            var operationResult = await sender.SendEmailAsync(_toEmailAddress, "EmailSender Basic Test", "Hello there");

            Assert.True(operationResult.IsSuccess);
        }

        [Fact]
        public void Test_template_parser()
        {
            string template = File.ReadAllText(_templateFilePath);

            ExampleData  data = ExampleData(_templateExampleData);
            object model = new { Model = data };

            var parser = new DotLiquidTemplateParser();
            var result = parser.Render(template, model);

            Assert.Contains("Mr Dillan Cagnetta", result);
        }

        [Fact]
        public async Task Test_send_templated_email()
        {
            string template = await File.ReadAllTextAsync(_templateFilePath);

            ExampleData data = ExampleData(_templateExampleData);
            object model = new { Model = data };

            var parser = new DotLiquidTemplateParser();
            var htmlBody = parser.Render(template, model);

            var sender = new AwsSesEmailSender(_awsAccessKeyId, _awsSecretAccessKey);

            var operationResult = await sender.SendEmailAsync(_toEmailAddress, "EmailSender Template Test", htmlBody);

            Assert.True(operationResult.IsSuccess);
        }

        private ExampleData ExampleData(string dataFilePath)
        {
            string dataFileContent = File.ReadAllText(dataFilePath);

            return JsonConvert.DeserializeObject<ExampleData>(dataFileContent);
        }
    }
}
