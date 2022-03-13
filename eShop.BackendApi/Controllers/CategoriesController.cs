using eShop.Application.Catalog.Categories;
using eShop.ViewModels.Catalog.ProductCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShop.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll(string languageId)
        {
            var product = await _categoryService.GetAll(languageId);
            return Ok(product);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetManageProductCategoryPagingRequest request)
        {
            var product = await _categoryService.GetAllPaging(request);
            return Ok(product);
        }

        [HttpGet("{productCategoryId}/{languageId}")]
        public async Task<IActionResult> GetById(int productCategoryId, string languageId)
        {
            var category = await _categoryService.GetById(productCategoryId, languageId);
            if (category == null)
                return BadRequest("Không tim thấy danh mục sản phẩm");
            return Ok(category);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] ProductCategoryCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var categoryId = await _categoryService.Create(request);
            if (categoryId == 0)
                return BadRequest();//400

            var product = await _categoryService.GetById(categoryId, request.LanguageId);

            return CreatedAtAction(nameof(GetById), new { id = categoryId }, product);
        }

        [HttpPut("{categoryId}")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int categoryId, [FromForm] ProductCategoryUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            request.CategoryId = categoryId;
            var affectedResult = await _categoryService.Update(request);
            if (affectedResult == 0)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("{categoryId}")]
        [Authorize]
        public async Task<IActionResult> Delete(int categoryId)
        {
            var affectedResult = await _categoryService.Delete(categoryId);
            if (affectedResult == 0)
                return BadRequest();

            return Ok();
        }
    }
}