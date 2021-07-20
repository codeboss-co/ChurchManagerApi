using ChurchManager.Domain.Features.Groups;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1.Crud
{
    [ApiVersion("1.0")]
    [Authorize]
    public partial class GroupTypesController : BaseCrudApiController<GroupType>
    {
    }
}
