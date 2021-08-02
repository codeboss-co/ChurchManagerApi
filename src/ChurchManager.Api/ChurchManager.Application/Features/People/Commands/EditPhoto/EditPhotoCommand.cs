using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Domain.Features.People.Services;
using CodeBoss.Extensions;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ChurchManager.Application.Features.People.Commands.EditPhoto
{
    public record EditPhotoCommand(int PersonId, IFormFile File) : IRequest;

    public class EditPhotoHandler : IRequestHandler<EditPhotoCommand>
    {
        private readonly IPhotoService _photos;
        private readonly IPersonDbRepository _dbRepository;
        private readonly string _environment;

        public EditPhotoHandler(
            IPhotoService photos, 
            IPersonDbRepository dbRepository,
            IWebHostEnvironment host)
        {
            _photos = photos;
            _dbRepository = dbRepository;
            _environment = host.EnvironmentName;
        }

        public async Task<Unit> Handle(EditPhotoCommand command, CancellationToken ct)
        {
            var person = await _dbRepository.GetByIdAsync(command.PersonId, ct);

            if (person is not null)
            {
                var fileName = $"{person.Id}-{person.FullName.FirstName}-{person.FullName.LastName}-{_environment}";

                // Delete current photo
                if(!person.PhotoUrl.IsNullOrEmpty() && person.PhotoUrl.Contains("cloudinary"))
                {
                    // https://res.cloudinary.com/codebossza/image/upload/v1627875380/Development/lnaughtycscssronmncu.png
                    var parsed = person.PhotoUrl.Substring(8); // remove https://
                    parsed = parsed.Remove(parsed.Length - 4); // remove file extension e.g. .png
                    var split = parsed.Split("/");
                    var splitLength = split.Length;
                    var publicId = $"{split[splitLength - 2]}/{split[splitLength-1]}";

                    await _photos.DeletePhotoAsync(publicId);
                }

                var operationResult = await _photos.AddPhotoAsync(fileName, command.File, ct);

                if (operationResult.IsSuccess)
                {
                    // The new image URL
                    person.PhotoUrl = operationResult.Result;
                    await _dbRepository.SaveChangesAsync(ct);
                }
            }

            return Unit.Value;
        }
    }
}
