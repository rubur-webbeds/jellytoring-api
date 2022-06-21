using jellytoring_api.Models.Images;
using jellytoring_api.Service.Inference;
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
    public class InferencesController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly InferenceService _inferenceService;

        public InferencesController(IHttpContextAccessor httpContextAccessor, InferenceService inferenceService)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _inferenceService = inferenceService ?? throw new ArgumentNullException(nameof(inferenceService));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Image image)
        {
            if (image is null)
            {
                return BadRequest();
            }
            var userEmail = _httpContextAccessor.HttpContext.Items["UserEmail"].ToString();
            var inferenceId = await _inferenceService.CreateAsync(userEmail, image);
            Console.WriteLine($"InferenceId: {inferenceId}");

            return inferenceId > 0 ? CreatedAtAction(nameof(Post), inferenceId) : StatusCode(500);
        }

        [HttpGet("{inferenceId}")]
        public async Task<IActionResult> Get(int inferenceId)
        {
            var result = await _inferenceService.GetAsync(inferenceId);

            return Ok(result);
        }
    }
}
