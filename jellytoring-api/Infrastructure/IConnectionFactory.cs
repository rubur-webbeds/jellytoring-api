using System.Data.Common;

namespace jellytoring_api.Infrastructure
{
    public interface IConnectionFactory
    {
        DbConnection CreateConnection();
    }
}
