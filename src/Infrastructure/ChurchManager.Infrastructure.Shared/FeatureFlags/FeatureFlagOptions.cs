namespace ChurchManager.Infrastructure.Shared.FeatureFlags
{
    public class FeatureFlagOptions
    {
        public bool Enabled { get; set; }
        public string ApiUrl { get; set; }
        public string Key { get; set; }
    }
}
