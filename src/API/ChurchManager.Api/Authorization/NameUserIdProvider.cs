using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace ChurchManager.Api.Authorization
{
    /// <summary>
    ///  Change to use name as the user identifier for SignalR
    /// </summary>
    public class NameUserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
