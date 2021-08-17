using DotLiquid;
using DotLiquid.NamingConventions;


namespace ChurchManager.Infrastructure.Shared.Templating
{
    public class DotLiquidTemplateParser
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
        /// 
        /// </summary>
        /// <param name="template">"Hello @Model.Name, welcome to RazorEngine!"</param>
        /// <param name="model">new { Model = data }</param>
        /// <returns></returns>
        public string Render(string liquidTemplateContent, object model)
        {
            Template template = Template.Parse(liquidTemplateContent);
            template.MakeThreadSafe();

            Hash hash = Hash.FromAnonymousObject(model);

            return template.Render(hash);
        }

        public string Render<TModel>(string template, TModel model)
        {
            return null;
        }
    }
}
