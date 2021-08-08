using jellytoring_api.Models.Users;
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
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(uint id)
        {
            var user = await _usersService.GetAsync(id);

            return user is not null ? Ok(user) : NotFound();
        }

        [HttpPost]
        /// <param name="user"></param>
        public async Task<ActionResult<User>> PostUser([FromBody] CreateUser user)
        {
            var newUser = await _usersService.CreateAsync(user);

            return newUser is not null ? CreatedAtAction(nameof(PostUser), newUser) :  StatusCode(500, "Oops! Something went wrong");
        }
    }
}
