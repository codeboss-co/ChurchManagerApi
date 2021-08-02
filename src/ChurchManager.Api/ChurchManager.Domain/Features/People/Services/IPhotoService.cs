using System.Threading;
using System.Threading.Tasks;
using Codeboss.Results;
using Microsoft.AspNetCore.Http;

namespace ChurchManager.Domain.Features.People.Services
{
    public interface IPhotoService
    {
        Task<OperationResult<string>> AddPhotoAsync(string fileName, IFormFile file, CancellationToken ct = default);
        Task<OperationResult<string>> DeletePhotoAsync(string publicId);
    }
}
