using System.Threading.Tasks;
using API._Helpers;
using API._Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace API._Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );


            this.cloudinary = new Cloudinary(acc);

        }

        public async Task<ImageUploadResult> AddCloudPhotoAsync(IFormFile file)
        {
            ImageUploadResult uploadResult = null;

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var imageUploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                };

                uploadResult = await this.cloudinary.UploadAsync(imageUploadParams);
            }

            return uploadResult;

        }
    }
}