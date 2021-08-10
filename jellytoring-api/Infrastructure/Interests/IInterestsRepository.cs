using jellytoring_api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jellytoring_api.Infrastructure.Interests
{
    public interface IInterestsRepository
    {
        Task<IEnumerable<Interest>> GetAllAsync();
    }
}