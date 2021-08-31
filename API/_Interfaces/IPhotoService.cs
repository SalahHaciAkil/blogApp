using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace API._Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddCloudPhotoAsync(IFormFile file);
    }
}