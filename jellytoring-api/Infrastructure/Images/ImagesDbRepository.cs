using Dapper;
using jellytoring_api.Models.Images;
using System.Collections.Generic;
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

            var result = await connection.QueryFirstOrDefaultAsync(ImagesQueries.Get, new { imageId });
            return Slapper.AutoMapper.MapDynamic<Image>(result);
        }

        public async Task<uint> CreateAsync(uint userId, Image image)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var param = new { userId, image.Location, image.Filename, image.Date, StatusId = image.Status.Id };
            return await connection.ExecuteScalarAsync<uint>(ImagesQueries.Create, param);
        }

        public async Task<IEnumerable<Image>> GetUserImagesAsync(uint userId)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var result = await connection.QueryAsync(ImagesQueries.GetUserImages, new { userId });
            return Slapper.AutoMapper.MapDynamic<Image>(result);
        }
    }
}
