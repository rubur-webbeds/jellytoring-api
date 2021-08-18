using jellytoring_api.Models.Images;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace jellytoring_api.Infrastructure.Images
{
    public class ImagesDiskRepository : IImagesDiskRepository
    {
        private readonly IConfiguration _configuration;
        public ImagesDiskRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SaveAsync(Image image)
        {
            if (image.File.Length > 0)
            {
                var filePath = _configuration.GetValue<string>("ImagesFilePath");

                using (var stream = File.Create(Path.Combine(filePath, image.Filename)))
                {
                    await image.File.CopyToAsync(stream);
                    stream.Close();
                }
            }
        }
    }
}
