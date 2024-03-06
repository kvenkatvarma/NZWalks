using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Api.Models.Domain;
using NZWalks.Api.Models.DTO;
using NZWalks.Api.Repositories;

namespace NZWalks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        public ImagesController(IImageRepository imageRepository)
        {
            ImageRepository = imageRepository;
        }

        public IImageRepository ImageRepository { get; }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> upload([FromForm] ImageUploadRequestDto request)
        {
            ValidateFileUpload(request);
            if(ModelState.IsValid)
            {
                var imagemodel = new Image
                {
                    File = request.File,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length,
                    FileName = request.FileName,
                    FileDescription = request.FileDescription
                };
                await ImageRepository.Upload(imagemodel);
                return Ok(imagemodel);
            }
            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            var allowedExtensions = new string[] { ".jpg", ",jpeg", ".png" };
            if(!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("File", "unsupported file extension");
            }
            if(request.File.Length >10485760)
            {
                ModelState.AddModelError("file", "File size more than 10 MB");
            }
        }
    }
}
