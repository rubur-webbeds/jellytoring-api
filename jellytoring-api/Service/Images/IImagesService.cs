using jellytoring_api.Models.Images;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jellytoring_api.Service.Images
{
    public interface IImagesService
    {
        Task<Image> CreateAsync(string userEmail, Image image);
        Task<Image> GetAsync(uint imageId);
        Task<IEnumerable<Image>> GetAllAsync(ImagesFilter filter);
        Task<IEnumerable<Image>> GetUserImagesAsync(string userEmail);
        Task<Image> ResolveAsync(ImageResolution image);
    }
}