using Dapper;
using jellytoring_api.Models.PasswordRecovery;
using System.Threading.Tasks;

namespace jellytoring_api.Infrastructure.PasswordRecoveries
{
    public class PasswordRecoveryRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public PasswordRecoveryRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<uint> CreateConfirmationAsync(PasswordResetConfirmation passwordReset)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            return await connection.ExecuteScalarAsync<uint>(PasswordRecoveryQueries.Create, passwordReset);
        }

        public async Task<bool> UpdatePasswordAsync(NewPasswordRequest passwordRequest)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            var updatePasswordCommmand = connection.CreateCommand();
            var confirmationCodeParam = updatePasswordCommmand.CreateParameter();
            var passwordParam = updatePasswordCommmand.CreateParameter();

            confirmationCodeParam.ParameterName = "@confirmationCode";
            confirmationCodeParam.Value = passwordRequest.ConfirmationCode;

            passwordParam.ParameterName = "@password";
            passwordParam.Value = passwordRequest.Password;

            updatePasswordCommmand.CommandText = PasswordRecoveryQueries.UpdatePassword;
            updatePasswordCommmand.Parameters.Add(confirmationCodeParam);
            updatePasswordCommmand.Parameters.Add(passwordParam);

            var rows = await updatePasswordCommmand.ExecuteNonQueryAsync();

            return rows == 2;
        }

        public async Task<PasswordResetConfirmation> GetConfirmationAsync(string confirmationCode)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            return await connection.QueryFirstOrDefaultAsync<PasswordResetConfirmation>(PasswordRecoveryQueries.Get, new { confirmationCode });
        }
    }
}
