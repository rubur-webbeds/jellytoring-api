using jellytoring_api.Infrastructure.Countries;
using jellytoring_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jellytoring_api.Service.Countries
{
    public class CountriesService : ICountriesService
    {
        private readonly ICountriesRepository _countriesRepository;

        public CountriesService(ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;
        }

        public Task<IEnumerable<Country>> GetAllAsync() => _countriesRepository.GetAllAsync();
    }
}
