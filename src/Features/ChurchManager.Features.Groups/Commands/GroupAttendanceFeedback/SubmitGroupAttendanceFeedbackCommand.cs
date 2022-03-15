using System.ComponentModel.DataAnnotations;
using ChurchManager.Domain.Features.Groups;
using ChurchManager.Domain.Features.Groups.Events;
using ChurchManager.Domain.Features.Groups.Repositories;
using ChurchManager.SharedKernel.Common;
using MediatR;

namespace ChurchManager.Features.Groups.Commands.GroupAttendanceFeedback
{
    public record SubmitGroupAttendanceFeedbackCommand : IRequest
    {
        [Required] public int AttendanceId { get; set; }

        public string Feedback { get; set; }
    }

    public class GroupAttendanceFeedbackHandler : IRequestHandler<SubmitGroupAttendanceFeedbackCommand>
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

        public async Task<Unit> Handle(SubmitGroupAttendanceFeedbackCommand command, CancellationToken ct)
        {
            var attendance = await _dbRepository.GetByIdAsync(command.AttendanceId);

            if (attendance.AttendanceReview is null)
            {
                attendance.AttendanceReview = new AttendanceReview
                {
                    IsReviewed = true,
                    Feedback = command.Feedback,
                    ReviewedBy = _currentUser.Username
                };
            }
            else
            {
                attendance.AttendanceReview.IsReviewed = true;
                attendance.AttendanceReview.Feedback = command.Feedback;
                attendance.AttendanceReview.ReviewedBy = _currentUser.Username;
            }

            attendance.AddDomainEvent(new GroupAttendanceReviewedEvent(attendance.Id, attendance.GroupId)
            {
                Feedback = command.Feedback
            });

            await _dbRepository.SaveChangesAsync(ct);

            return new Unit();
        }
    }
}