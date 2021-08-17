using jellytoring_api.Models.Images;
using jellytoring_api.Service.Images;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace jellytoring_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImagesService _imagesService;

        public ImagesController(IImagesService imagesService)
        {
            _imagesService = imagesService;
        }

        [HttpPost("user/{userId}")]
        public async Task<ActionResult<Image>> Post(uint userId, [FromForm] Image image)
        {
            if(image is null)
            {
                return BadRequest();
            }

            image.UserId = userId;

            var createImage = await _imagesService.CreateAsync(userId, image);

            return createImage is not null ? CreatedAtAction(nameof(Post), createImage) : StatusCode(500);
        }
    }
}
