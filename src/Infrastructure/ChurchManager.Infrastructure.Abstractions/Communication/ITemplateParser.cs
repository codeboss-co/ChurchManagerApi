namespace ChurchManager.Infrastructure.Abstractions.Communication
{
    public interface ITemplateParser
    {
        string Render(string template, object model);
    }
}
