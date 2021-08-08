using System.Data.Common;

namespace jellytoring_api.Infrastructure
{
    public class MySqlConnectionFactory : IConnectionFactory
    {
        private readonly string _connectionString;
        public MySqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }
        public DbConnection CreateConnection()
        {
            return new MySqlConnector.MySqlConnection(_connectionString);
        }
    }
}
