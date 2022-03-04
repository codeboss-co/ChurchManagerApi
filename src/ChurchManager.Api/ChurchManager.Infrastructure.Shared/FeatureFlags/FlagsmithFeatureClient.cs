using ChurchManager.Infrastructure.Abstractions.FeatureFlags;
using DotLiquid.Util;
using Flagsmith;

namespace ChurchManager.Infrastructure.Shared.FeatureFlags
{
    public class FlagsmithFeatureClient : IFeatureFlagClient
    {
        public async Task<bool?> HasFeatureFlag(string featureId, string identity = null)
        {
            var featureEnabled = await FlagsmithClient.instance.HasFeatureFlag(featureId, identity);

            return featureEnabled.IsTruthy();
        }

        public async Task<string> GetFeatureValue(string featureId, string identity = null)
        {
            return await FlagsmithClient.instance.GetFeatureValue(featureId, identity);
        }
    }
}
