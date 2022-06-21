using Dapper;
using jellytoring_api.Infrastructure.Images;
using jellytoring_api.Models.Images;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace jellytoring_api.Infrastructure.Inference
{
    public class InferenceRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IImagesDiskRepository _imagesDiskRepository;
        private readonly IConfiguration _configuration;

        public InferenceRepository(
            IConnectionFactory connectionFactory,
            IImagesDiskRepository imagesDiskRepository,
            IConfiguration configuration)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _imagesDiskRepository = imagesDiskRepository ?? throw new ArgumentNullException(nameof(imagesDiskRepository));
            _configuration = configuration;
        }
        public async Task<(uint inferenceId, string filePath)> CreateAsync(uint userId, Image image)
        {
            // create record in inferences table
            var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var inference = new
            {
                StartedAt = DateTime.Now,
                ImageName = image.Filename,
                UserId = userId,
                Status = "RUNNING"
            };
            var inferenceId = await connection.ExecuteScalarAsync<uint>(InferenceQueries.Create, inference);
            if (inferenceId <= 0)
                return default;

            // add the folder structure in the filename
            var now = DateTime.Now.ToString("ddMMyyyy-HHmmss");
            var basePath = _configuration.GetValue<string>("ImagesFilePath");
            var relativePath = Path.Combine("testdir", $"{userId}", now);
            var filePath = Path.Combine(basePath, relativePath);

            // save image in disk
            await _imagesDiskRepository.SaveAsync(image, filePath);

            return (inferenceId, relativePath);
        }

        public async Task<string> MarkAsCompletedAsync(string originalImageName, string outputPath)
        {
            var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var param = new { originalImageName, outputPath };
            var affectedRows = await connection.ExecuteScalarAsync<uint>(InferenceQueries.MarkAsCompleted, param);

            return affectedRows == 1 ? outputPath : "error";
        }

        public async Task<string> GetAsync(int inferenceId)
        {
            var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var param = new { inferenceId };
            var (Status, OutputPath) = await connection.QuerySingleAsync<(string Status, string OutputPath)>(InferenceQueries.Get, param);

            return Status == "RUNNING" ? Status : OutputPath;
        }
    }
}
