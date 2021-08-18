using Dapper;
using jellytoring_api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jellytoring_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConnectionFactory _connectionFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IConnectionFactory connectionFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var userEmail = (string)_httpContextAccessor.HttpContext.Items["UserEmail"];
            return Ok(userEmail);
        }

        [HttpGet("/roles")]
        public async Task<IActionResult> GetRoles()
        {
            IEnumerable<string> roles;
            using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();
            //var command = connection.CreateCommand();
            //command.CommandText = "select name from roles";

            //var result = await command.ExecuteReaderAsync();

            //while (result.Read())
            //{
            //    roles.Add(result.GetString(0));
            //}

            roles = await connection.QueryAsync<string>("select name from roles");

            return Ok(roles);
        }
    }
}
