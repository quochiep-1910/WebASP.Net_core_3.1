using eShop.Application.Catalog.Categories;
using eShop.ViewModels.Catalog.ProductCategory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}