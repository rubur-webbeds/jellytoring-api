using jellytoring_api.Models.Images;
using System.Threading.Tasks;

namespace jellytoring_api.Infrastructure.Images
{
    public interface IImagesDbRepository
    {
        Task<uint> CreateAsync(uint userId, Image image);
        Task<Image> GetAsync(uint imageId);
    }
}