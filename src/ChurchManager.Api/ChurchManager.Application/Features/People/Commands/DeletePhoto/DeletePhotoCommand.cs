using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Application.Extensions;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Features.People.Services;
using CodeBoss.Extensions;
using MediatR;

namespace ChurchManager.Application.Features.People.Commands.DeletePhoto
{
    public record DeletePhotoCommand(int PersonId) : IRequest;

    public class DeletePhotoHandler : IRequestHandler<DeletePhotoCommand>
    {
        private readonly IPhotoService _photos;
        private readonly IPersonDbRepository _dbRepository;

        public DeletePhotoHandler(
            IPhotoService photos,
            IPersonDbRepository dbRepository)
        {
            _photos = photos;
            _dbRepository = dbRepository;
        }

        public async Task<Unit> Handle(DeletePhotoCommand command, CancellationToken ct)
        {
            var person = await _dbRepository.GetByIdAsync(command.PersonId, ct);

            if (person is not null)
            {
                // Delete current photo
                if (!person.PhotoUrl.IsNullOrEmpty() && person.PhotoUrl.Contains("cloudinary"))
                {
                    var publicId = person.CloudinaryPublicId();
                    await _photos.DeletePhotoAsync(publicId);
                }

                // Reset person photo
                person.PhotoUrl = null;
                await _dbRepository.SaveChangesAsync(ct);
            }

            return Unit.Value;
        }
    }
}
