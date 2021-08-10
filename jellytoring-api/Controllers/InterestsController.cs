using jellytoring_api.Models;
using jellytoring_api.Service.Interests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jellytoring_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterestsController : ControllerBase
    {
        private readonly IInterestsService _interestsService;

        public InterestsController(IInterestsService interestsService)
        {
            _interestsService = interestsService;
        }

        [HttpGet]
        public async Task<IEnumerable<Interest>> GetAll()
        {
            return await _interestsService.GetAllAsync();
        }
    }
}
