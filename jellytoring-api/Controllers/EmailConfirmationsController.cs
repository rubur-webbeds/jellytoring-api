using jellytoring_api.Service.Email;
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
    public class EmailConfirmationsController : ControllerBase
    {
        private readonly IEmailConfirmationService _emailConfirmationService;

        public EmailConfirmationsController(IEmailConfirmationService emailConfirmationService)
        {
            _emailConfirmationService = emailConfirmationService;
        }

        [HttpGet("{confirmationCode}")]
        public async Task<IActionResult> Get(string confirmationCode)
        {
            var resultOk = await _emailConfirmationService.ConfirmEmailAsync(confirmationCode);
            return resultOk ? Ok() : StatusCode(500);
        }
    }
}
