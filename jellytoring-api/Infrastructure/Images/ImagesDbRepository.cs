using Dapper;
using jellytoring_api.Models;
using jellytoring_api.Models.Images;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jellytoring_api.Infrastructure.Images
{
    public class ImagesDbRepository : IImagesDbRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly SqlBuilder _sqlBuilder;

        public ImagesDbRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _sqlBuilder = new SqlBuilder();
        }
        public async Task<Image> GetAsync(uint imageId)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var builderTemplate = _sqlBuilder.AddTemplate(ImagesQueries.GetAll);

            _sqlBuilder.Where(ImagesQueries.GetByImageId, new { imageId });

            var result = await connection.QueryFirstOrDefaultAsync(builderTemplate.RawSql, builderTemplate.Parameters);
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

            var builderTemplate = _sqlBuilder.AddTemplate(ImagesQueries.GetAll);

            _sqlBuilder.Where(ImagesQueries.GetByUserId, new { userId });

            var result = await connection.QueryAsync(builderTemplate.RawSql, builderTemplate.Parameters);
            return Slapper.AutoMapper.MapDynamic<Image>(result);
        }

        public async Task<IEnumerable<Image>> GetAllAsync(ImagesFilter filter)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var builderTemplate = _sqlBuilder.AddTemplate(ImagesQueries.GetAll);

            if(!string.IsNullOrEmpty(filter.StatusCode))
            {
                _sqlBuilder.Where("statuses.code = @StatusCode", filter);
            }

            var result = await connection.QueryAsync(builderTemplate.RawSql, builderTemplate.Parameters);
            return Slapper.AutoMapper.MapDynamic<Image>(result);
        }

        public async Task<bool> UpdateStatusAsync(uint imageId, Status status)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var statusId = status.Id;

            var command = connection.CreateCommand();
            var statusIdParam = command.CreateParameter();
            statusIdParam.ParameterName = "@statusId";
            statusIdParam.Value = statusId;
            var imageIdParam = command.CreateParameter();
            imageIdParam.ParameterName = "@imageId";
            imageIdParam.Value = imageId;
            command.CommandText = ImagesQueries.UpdateStatus;
            command.Parameters.Add(statusIdParam);
            command.Parameters.Add(imageIdParam);
            var rows = await command.ExecuteNonQueryAsync();

            return rows == 1;
        }
    }
}
