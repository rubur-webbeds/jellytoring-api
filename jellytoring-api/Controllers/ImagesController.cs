using jellytoring_api.Models.Images;
using jellytoring_api.Service.Images;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace jellytoring_api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImagesService _imagesService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImagesController(IImagesService imagesService, IHttpContextAccessor httpContextAccessor)
        {
            _imagesService = imagesService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<ActionResult<Image>> Post([FromForm] Image image)
        {
            if(image is null)
            {
                return BadRequest();
            }
            var userEmail = _httpContextAccessor.HttpContext.Items["UserEmail"].ToString();
            var createImage = await _imagesService.CreateAsync(userEmail, image);

            return createImage is not null ? CreatedAtAction(nameof(Post), createImage) : StatusCode(500);
        }
    }
}
