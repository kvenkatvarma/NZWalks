using NZWalks.Api.Data;
using NZWalks.Api.Models.Domain;

namespace NZWalks.Api.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        public LocalImageRepository(IWebHostEnvironment webHostEnvironment,IHttpContextAccessor contextaccessor,NZWalksDbContext nZWalksDbContext)
        {
            WebHostEnvironment = webHostEnvironment;
            Contextaccessor = contextaccessor;
            NZWalksDbContext = nZWalksDbContext;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }
        public IHttpContextAccessor Contextaccessor { get; }
        public NZWalksDbContext NZWalksDbContext { get; }

        public async Task<Image> Upload(Image image)
        {
            var localFilePath = Path.Combine(WebHostEnvironment.ContentRootPath, "Images", $"{image.FileName}{image.FileExtension}");
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);
            var urlFilePath = $"{Contextaccessor.HttpContext.Request.Scheme}://{Contextaccessor.HttpContext.Request.Host}{Contextaccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;
            await NZWalksDbContext.Images.AddAsync(image);//Adding the image to database
            await NZWalksDbContext.SaveChangesAsync();
            return image;
        }
    }
}
