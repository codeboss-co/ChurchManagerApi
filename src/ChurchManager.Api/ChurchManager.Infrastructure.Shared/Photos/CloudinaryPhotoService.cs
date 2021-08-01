using System;
using System.Threading;
using System.Threading.Tasks;
using ChurchManager.Domain.Features.People.Services;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Codeboss.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ChurchManager.Infrastructure.Shared.Photos
{
    public class CloudinaryPhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryPhotoService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<OperationResult<string>> AddPhotoAsync(
            string fileName, IFormFile file, CancellationToken ct = default)
        {
            var result = new ImageUploadResult();

            if(file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription($"{fileName}", stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };
                result = await _cloudinary.UploadAsync(uploadParams, ct);
            }

            return CheckResult(result);
        }

        public async Task<OperationResult<string>> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);

            var result = await _cloudinary.DestroyAsync(deleteParams);

            return CheckResult(result);
        }

        private OperationResult<string> CheckResult(BaseResult result)
        {
            if(result.Error is not null)
            {
                return OperationResult<string>.Fail(result.Error.Message);
            }

            return result switch
            {
                ImageUploadResult x => new OperationResult<string>(true, x.SecureUrl.AbsoluteUri),
                DeletionResult => OperationResult<string>.Success(string.Empty),
                _ => throw new InvalidOperationException("Invalid response parsed in switch")
            };
        }
    }
}
