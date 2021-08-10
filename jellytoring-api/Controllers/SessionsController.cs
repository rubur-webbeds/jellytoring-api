using jellytoring_api.Service.Users;
using jellytoring_api.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace jellytoring_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly ISessionsService _sessionsService;

        public SessionsController(ISessionsService sessionsService)
        {
            _sessionsService = sessionsService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<User>> PostSession([FromBody] CreateSessionUser createSessionUser)
        {
            var jwt = await _sessionsService.CreateAsync(createSessionUser);
            return jwt is not null ? CreatedAtAction(nameof(PostSession), jwt) : StatusCode(500, "Oops! Something went wrong");
        }

        // FOR TESTING ONLY
        [HttpGet]
        [Authorize]
        public ActionResult<string> GetUser()
        {
            return Ok("Authorize Endpoit 200 return");
        }
    }
}