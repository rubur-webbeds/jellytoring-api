using jellytoring_api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jellytoring_api.Infrastructure.Statuses
{
    public interface IStatusesRepository
    {
        Task<IEnumerable<Status>> GetAllAsync();
    }
}