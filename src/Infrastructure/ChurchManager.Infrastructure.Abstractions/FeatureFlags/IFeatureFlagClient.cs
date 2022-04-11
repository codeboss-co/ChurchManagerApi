namespace ChurchManager.Infrastructure.Abstractions.FeatureFlags
{
    public interface IFeatureFlagClient
    {
        Task<bool?> HasFeatureFlag(string featureId, string identity = null);
        Task<string> GetFeatureValue(string featureId, string identity = null);
    }
}
