using ChurchManager.Infrastructure.Abstractions.Communication;
using DotLiquid;
using DotLiquid.NamingConventions;


namespace ChurchManager.Infrastructure.Shared.Templating
{
    // https://github.com/JonJam/templates_dotliquid/blob/master/TemplatesWithDotLiquid/Program.cs
    public class DotLiquidTemplateParser : ITemplateParser
    {
        public DotLiquidTemplateParser()
        {
            Liquid.UseRubyDateFormat = false;
            // Setting this means that:
            // - Properties are accessed using CamelCase e.g. Model.PolicyNumber
            // - Filters are accessed using CamelCase e.g. Date
            Template.NamingConvention = new CSharpNamingConvention();
        }

        /// <summary>
        /// Render template
        /// </summary>
        /// <param name="model">new { Model = data }</param>
        /// <returns></returns>
        public string Render(string liquidTemplateContent, object model)
        {
            Template template = Template.Parse(liquidTemplateContent);
            template.MakeThreadSafe();

            Hash hash = Hash.FromAnonymousObject(model);

            return template.Render(hash);
        }
    }
}
