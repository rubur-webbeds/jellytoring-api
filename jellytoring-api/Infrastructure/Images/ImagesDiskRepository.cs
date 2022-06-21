using jellytoring_api.Models.Images;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace jellytoring_api.Infrastructure.Images
{
    public class ImagesDiskRepository : IImagesDiskRepository
    {
        public async Task SaveAsync(Image image, string path)
        {
            if (image.File.Length > 0)
            {
                var fullPath = Path.Combine(path, image.Filename);

                Directory.CreateDirectory(path);

                using var stream = File.Create(fullPath);
                await image.File.CopyToAsync(stream);
            }
        }
    }
}
