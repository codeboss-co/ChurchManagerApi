using ChurchManager.Application.Wrappers;
using MediatR;

namespace ChurchManager.Application.Features.Discipleship.Queries.DiscipleshipTypesAndStepDefinitions
{
    public record DiscipleshipProgramsQuery : IRequest<ApiResponse>
    {
    }
}