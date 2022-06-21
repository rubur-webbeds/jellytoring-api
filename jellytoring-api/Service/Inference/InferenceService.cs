using jellytoring_api.Infrastructure.Images;
using jellytoring_api.Infrastructure.Inference;
using jellytoring_api.Infrastructure.Users;
using jellytoring_api.Models.Images;
using jellytoring_api.Service.Images;
using jellytoring_api.Service.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace jellytoring_api.Service.Inference
{
    public class InferenceService
    {
        private readonly InferenceEngine _inferenceEngine;
        private readonly InferenceRepository _inferenceRepository;
        private readonly IUsersRepository _usersRepository;

        public InferenceService(
            InferenceEngine inferenceEngine,
            InferenceRepository inferenceRepository,
            IUsersRepository usersRepository,
            IImagesDiskRepository imagesDiskRepository)
        {
            _inferenceEngine = inferenceEngine ?? throw new ArgumentNullException(nameof(inferenceEngine));
            _inferenceRepository = inferenceRepository ?? throw new ArgumentNullException(nameof(inferenceRepository));
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
        }

        public async Task<uint> CreateAsync(string email, Image image)
        {
            if (!ImagesService.Validate(image))
            {
                return 0;
            }

            var user = await _usersRepository.GetAsync(email);

            var newFilename = Guid.NewGuid();
            var extension = ImagesService.ContentTypeToExtension(image.File.ContentType);
            if (extension == "error")
            {
                return 0;
            }

            image.Filename = $"{newFilename}.{extension}";

            var (inferenceId, filePath) = await _inferenceRepository.CreateAsync(user.Id, image);

            // send to inference engine
            _ = Task.Run(() => _inferenceEngine.Send(filePath.Replace('\\', '/')));

            return inferenceId;
        }

        public Task<string> GetAsync(int inferenceId) => _inferenceRepository.GetAsync(inferenceId);
    }
}
