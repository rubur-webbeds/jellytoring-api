using jellytoring_api.Models.Images;
using System.Threading.Tasks;

namespace jellytoring_api.Service.Images
{
    public interface IImagesService
    {
        Task<Image> CreateAsync(uint userId, Image image);
        Task<Image> GetAsync(uint imageId);
    }
}