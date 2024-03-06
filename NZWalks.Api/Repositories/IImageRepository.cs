using NZWalks.Api.Models.Domain;

namespace NZWalks.Api.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
