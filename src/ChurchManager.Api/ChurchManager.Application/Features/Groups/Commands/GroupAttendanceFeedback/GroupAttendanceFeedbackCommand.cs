using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.Domain.Model.Groups;
using MediatR;

namespace ChurchManager.Application.Features.Groups.Commands.AttendanceFeedback
{
    public record GroupAttendanceFeedbackCommand : IRequest
    {
        [Required]
        public int AttendanceId { get; set; }
        public string Feedback { get; set; }
    }

    public class GroupAttendanceFeedbackHandler : IRequestHandler<GroupAttendanceFeedbackCommand>
    {
        private readonly ICognitoCurrentUser _currentUser;
        private readonly IGroupAttendanceDbRepository _dbRepository;

        public GroupAttendanceFeedbackHandler(
            ICognitoCurrentUser currentUser,
            IGroupAttendanceDbRepository dbRepository)
        {
            _currentUser = currentUser;
            _dbRepository = dbRepository;
        }

        public async Task<Unit> Handle(GroupAttendanceFeedbackCommand command, CancellationToken ct)
        {
            GroupAttendance attendance = await _dbRepository.GetByIdAsync(command.AttendanceId);

            if (attendance.AttendanceReview is null)
            {
                attendance.AttendanceReview = new AttendanceReview
                {
                    IsReviewed = true,
                    Feedback = command.Feedback,
                    ReviewedBy = _currentUser.Username,
                };
            }
            else
            {
                attendance.AttendanceReview.IsReviewed = true;
                attendance.AttendanceReview.Feedback = command.Feedback;
                attendance.AttendanceReview.ReviewedBy = _currentUser.Username;
            }

            await _dbRepository.SaveChangesAsync();

            return new Unit();
        }
    }
}
