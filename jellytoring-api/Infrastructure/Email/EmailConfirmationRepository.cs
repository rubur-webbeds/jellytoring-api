using Dapper;
using jellytoring_api.Models.Email;
using System.Threading.Tasks;

namespace jellytoring_api.Infrastructure.Email
{
    public class EmailConfirmationRepository : IEmailConfirmationRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public EmailConfirmationRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<uint> CreateConfirmationAsync(EmailConfirmation emailConfirmation)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            return await connection.ExecuteScalarAsync<uint>(EmailConfirmationQueries.Create, emailConfirmation);
        }
    }
}
