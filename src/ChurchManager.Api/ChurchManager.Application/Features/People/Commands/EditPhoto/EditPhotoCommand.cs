using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using MediatR;

namespace ChurchManager.Application.Features.People.Commands.EditPhoto
{
    public record EditPhotoCommand: IRequest
    {
        public int PersonId { get; set; }
        public string Base64FileString { get; set; }
    }

    public class EditPhotoHandler : IRequestHandler<EditPhotoCommand>
    {
        private readonly IAmazonS3 _amazonS3;

        public EditPhotoHandler(IAmazonS3 amazonS3)
        {
            _amazonS3 = amazonS3;
        }

        public Task<Unit> Handle(EditPhotoCommand command, CancellationToken ct)
        {
            throw new System.NotImplementedException();
        }

    }


}
