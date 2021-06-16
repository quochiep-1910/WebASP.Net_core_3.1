using eShop.Application.Utilities.Slides;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShop.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SlidesController : ControllerBase
    {
        public readonly ISlideService _slidesService;

        public SlidesController(ISlideService slidesService)
        {
            _slidesService = slidesService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var slides = await _slidesService.GetAll();
            return Ok(slides);
        }
    }
}