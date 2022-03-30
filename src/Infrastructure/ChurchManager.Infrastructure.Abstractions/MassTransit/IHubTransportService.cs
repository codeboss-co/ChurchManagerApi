namespace ChurchManager.Infrastructure.Abstractions.MassTransit
{
    public interface IHubTransportService<in TPublisher>
    {
        Task SendToUserAsync<TModel>(
            TModel model,
            string userId,
            string methodName,
            TPublisher publisher);
    }
}
