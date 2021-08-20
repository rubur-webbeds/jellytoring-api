using Dapper;
using jellytoring_api.Models.Images;
using System.Threading.Tasks;

namespace jellytoring_api.Infrastructure.Images
{
    public class ImagesDbRepository : IImagesDbRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public ImagesDbRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<Image> GetAsync(uint imageId)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            return await connection.QueryFirstOrDefaultAsync<Image>(ImagesQueries.Get, new { imageId });
        }

        public async Task<uint> CreateAsync(uint userId, Image image)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var param = new { userId, image.Location, image.Filename, image.Date, image.Confirmed };
            return await connection.ExecuteScalarAsync<uint>(ImagesQueries.Create, param);
        }
    }
}
