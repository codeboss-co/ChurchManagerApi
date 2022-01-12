using ChurchManager.Application.Wrappers;
using MediatR;

namespace ChurchManager.Application.Features.Missions.Queries.GetMission
{
    public record GetMissionQuery(int MissionId) : IRequest<ApiResponse>;
}
