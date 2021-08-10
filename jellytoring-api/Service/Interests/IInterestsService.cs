using jellytoring_api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jellytoring_api.Service.Interests
{
    public interface IInterestsService
    {
        Task<IEnumerable<Interest>> GetAllAsync();
    }
}