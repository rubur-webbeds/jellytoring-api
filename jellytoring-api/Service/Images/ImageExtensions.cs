using jellytoring_api.Models;
using jellytoring_api.Models.Images;

namespace jellytoring_api.Service.Images
{
    public static class ImageExtensions
    {
        public static void SetStatus(this Image image, Status status) => image.Status = status;
    }
}
