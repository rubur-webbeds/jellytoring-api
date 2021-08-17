using jellytoring_api.Infrastructure.Images;
using jellytoring_api.Infrastructure.Users;
using jellytoring_api.Models.Images;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace jellytoring_api.Service.Images
{
    public class ImagesService : IImagesService
    {
        private readonly IImagesDbRepository _imagesDbRepository;
        private readonly IImagesDiskRepository _imagesDiskRepository;
        private readonly IUsersRepository _usersRepository;

        public ImagesService(
            IImagesDbRepository imagesDbRepository,
            IImagesDiskRepository imagesDiskRepository,
            IUsersRepository usersRepository)
        {
            _imagesDbRepository = imagesDbRepository;
            _imagesDiskRepository = imagesDiskRepository;
            _usersRepository = usersRepository;
        }

        public async Task<Image> GetAsync(uint imageId)
        {
            var dbImage = await _imagesDbRepository.GetAsync(imageId);
            return dbImage;
        }

        public async Task<Image> CreateAsync(string userEmail, Image image)
        {
            /*
             * TODO: implement security validations
             * compare extension and contenttype
             * check validation signature
             */

            var user = await _usersRepository.GetAsync(userEmail);

            var newFilename = Guid.NewGuid();
            var extension = ContentTypeToExtension(image.File.ContentType);
            if(extension == "error")
            {
                return null;
            }

            image.Filename = $"{newFilename}.{extension}";

            var imageId = await _imagesDbRepository.CreateAsync(user.Id, image);

            if(imageId != 0)
            {
                await _imagesDiskRepository.SaveAsync(image);
            }

            return imageId != 0 ? await GetAsync(imageId) : null;
        }

        private string ContentTypeToExtension(string contentType)
        {
            switch (contentType)
            {
                case "image/jpeg":
                    return "jpeg";
                case "image/png":
                    return "png";
                default:
                    return "error";
            }
        }
    }
}
