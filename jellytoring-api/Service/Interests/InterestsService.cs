using jellytoring_api.Infrastructure.Interests;
using jellytoring_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jellytoring_api.Service.Interests
{
    public class InterestsService : IInterestsService
    {
        private readonly IInterestsRepository _interestsRepository;

        public InterestsService(IInterestsRepository interestsRepository)
        {
            _interestsRepository = interestsRepository;
        }

        public Task<IEnumerable<Interest>> GetAllAsync() => _interestsRepository.GetAllAsync();
    }
}
