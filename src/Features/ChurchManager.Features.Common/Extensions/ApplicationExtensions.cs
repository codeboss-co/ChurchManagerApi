using ChurchManager.Domain.Features.People;

namespace ChurchManager.Features.Common.Extensions
{
    public static class ApplicationExtensions
    {
        public static string CloudinaryPublicId(this Person person)
        {
            // https://res.cloudinary.com/codebossza/image/upload/v1627875380/Development/lnaughtycscssronmncu.png
            var parsed = person.PhotoUrl.Substring(8); // remove https://
            parsed = parsed.Remove(parsed.Length - 4); // remove file extension e.g. .png
            var split = parsed.Split("/");
            var splitLength = split.Length;
            var publicId = $"{split[splitLength - 2]}/{split[splitLength - 1]}";

            return publicId;
        }
    }
}
