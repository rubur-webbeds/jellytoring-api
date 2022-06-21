using jellytoring_api.Models.Images;
using System.Threading.Tasks;

namespace jellytoring_api.Infrastructure.Images
{
    public interface IImagesDiskRepository
    {
        Task SaveAsync(Image image, string path);
    }
}