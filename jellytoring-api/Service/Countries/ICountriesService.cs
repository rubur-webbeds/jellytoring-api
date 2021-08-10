using jellytoring_api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jellytoring_api.Service.Countries
{
    public interface ICountriesService
    {
        Task<IEnumerable<Country>> GetAllAsync();
    }
}