using jellytoring_api.Models.PasswordRecovery;
using jellytoring_api.Service.PasswordRecoveries;
using jellytoring_api.Service.Users;
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
    public class PasswordRecoveriesController : ControllerBase
    {
        private readonly PasswordRecoveryService _passwordRecoveryService;

        public PasswordRecoveriesController(
            PasswordRecoveryService passwordRecoveryService)
        {
            _passwordRecoveryService = passwordRecoveryService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string email)
        {
            var result = await _passwordRecoveryService.SendResetEmailTo(email);
            return result ? Ok() : StatusCode(500);
        }

        [HttpPost("{guid}")]
        public async Task<IActionResult> PostNewPassword(string guid, [FromBody] NewPasswordRequest passwordRequest)
        {
            var resultOk = await _passwordRecoveryService.UpdatePasswordAsync(passwordRequest);
            return resultOk ? Ok() : StatusCode(500);
        }
    }
}
