namespace ChurchManager.Infrastructure.Abstractions
{
    public interface IModelHelper
    {
        string ValidateModelFields<T>(string fields);
        string GetModelFields<T>();
    }
}