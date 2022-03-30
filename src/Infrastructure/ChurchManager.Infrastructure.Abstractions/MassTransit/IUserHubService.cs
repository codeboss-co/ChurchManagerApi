using MassTransit;

namespace ChurchManager.Infrastructure.Abstractions.MassTransit
{
    public interface IUserHubService
    {
        Task SendToUserAsync<TModel>(
            TModel model,
            string userId,
            string methodName,
            IPublishEndpoint publisher);
    }
}
