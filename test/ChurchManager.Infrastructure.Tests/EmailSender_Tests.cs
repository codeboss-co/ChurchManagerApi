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
        [Fact]
        public async Task Test_sending_emails()
        {
            var sender = new AwsSesEmailSender("AKIA6HLAUN2QAA2ZQ5W3", "PMd1zeIjRc2Nja0qwa+RzU3y5/xdqolEDODGQoqX");

            var operationResult = await sender.SendEmailAsync("dillancagnetta@yahoo.com", "EmailSender Tests", "Hello there");

            Assert.True(operationResult.IsSuccess);
        }

        [Fact]
        public void Test_template_parser()
        {
            string templateFilePath = "./Templates/ExampleTemplate.liquid";
            string template = File.ReadAllText(templateFilePath);

            ExampleData  data = ExampleData("./Templates/ExampleData.json");
            object model = new { Model = data };

            var parser = new DotLiquidTemplateParser();
            var result = parser.Render(template, model);

            Assert.Contains("Mr Dillan Cagnetta", result);
        }

        private ExampleData ExampleData(string dataFilePath)
        {
            string dataFileContent = File.ReadAllText(dataFilePath);

            return JsonConvert.DeserializeObject<ExampleData>(dataFileContent);
        }
    }
}
