using jellytoring_api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jellytoring_api.Infrastructure.Countries
{
    public interface ICountriesRepository
    {
        Task<IEnumerable<Country>> GetAllAsync();
    }
}