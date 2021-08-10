using jellytoring_api.Infrastructure.Users;
using jellytoring_api.Models.Users;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Newtonsoft.Json;

namespace jellytoring_api.Service.Users
{
    public class SessionsService : ISessionsService
    {
        private readonly IUsersRepository _usersRepository;
        private IConfiguration _config;

        public SessionsService(IUsersRepository usersRepository, IConfiguration config)
        {
            _usersRepository = usersRepository;
            _config = config;
        }

        public async Task<string> CreateAsync(CreateSessionUser createSessionUser)
        {
            var user = await _usersRepository.GetAsync(createSessionUser.Email);

            if (user == null || !BC.Verify(createSessionUser.Password, user.Password))
            {
                return null;
            } else {
                var userWithRoles = await _usersRepository.GetUserRolesAsync(createSessionUser.Email);
                return CreateJSONWebToken(userWithRoles);
            }
        }

        private string CreateJSONWebToken(SessionUser sessionUser)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims: new Claim[]
              {
                  new Claim("email", sessionUser.Email),
                  new Claim("full_name", sessionUser.FullName),
                  new Claim("roles", JsonConvert.SerializeObject(sessionUser.Roles))
              },
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
