using ChurchManager.SharedKernel.Wrappers;
using MediatR;

namespace ChurchManager.Features.Missions.Queries.GetMission
{
    public record GetMissionQuery(int MissionId) : IRequest<ApiResponse>;
}
