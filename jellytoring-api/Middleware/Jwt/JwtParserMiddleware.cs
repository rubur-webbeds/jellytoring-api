using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Middleware
{
    public class JwtParserMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public JwtParserMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token is not null)
            {
                attachUserInfoToContext(context, token);
            }

            await _next(context);
        }

        private void attachUserInfoToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userEmail = jwtToken.Claims.First(x => x.Type == "email").Value;
                var userFullName = jwtToken.Claims.First(x => x.Type == "full_name").Value;
                var userRoles = jwtToken.Claims.First(x => x.Type == "roles").Value;

                // attach user info to context on successful jwt validation
                context.Items["UserEmail"] = userEmail;
                context.Items["UserFullName"] = userFullName;
                context.Items["UserRoles"] = userRoles;
            }
            catch
            {
                // do nothing if jwt validation fails
                // user info is not attached to context so request won't have access to secure routes
            }
        }
    }
}