using eShop.Application.Catalog.Products;
using eShop.ViewModels.Catalog.ProductImages;
using eShop.ViewModels.Catalog.Products;
using eShop.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
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
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _ProductService;

        public ProductsController(IProductService ProductService)
        {
            _ProductService = ProductService;
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetManageProductPagingRequest request)
        {
            var product = await _ProductService.GetAllPaging(request);
            return Ok(product);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productId = await _ProductService.Create(request);
            if (productId == 0)
                return BadRequest();//400

            var product = await _ProductService.GetById(productId, request.LanguageId);

            return CreatedAtAction(nameof(GetById), new { id = productId }, product);
        }

        [HttpPut("{productId}")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int productId, [FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            request.Id = productId;
            var affectedResult = await _ProductService.Update(request);
            if (affectedResult == 0)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("{productId}")]
        [Authorize]
        public async Task<IActionResult> Delete(int productId)
        {
            var affectedResult = await _ProductService.Delete(productId);
            if (affectedResult == 0)
                return BadRequest();

            return Ok();
        }

        [HttpPatch("{productId}/{newPrice}")]
        [Authorize]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            var isSuccessful = await _ProductService.UpdatePrice(productId, newPrice);
            if (isSuccessful)
                return Ok();

            return BadRequest();
        }

        [HttpGet("{productId}/{languageId}")]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var product = await _ProductService.GetById(productId, languageId);
            if (product == null)
                return BadRequest("Không tim thấy product");
            return Ok(product);
        }

        [HttpPost("{productId}/images")]
        [Authorize]
        public async Task<IActionResult> CreateImage(int productId, [FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var imageId = await _ProductService.AddImage(productId, request);
            if (imageId == 0)
                return BadRequest();//400

            var image = await _ProductService.GetImageById(imageId);

            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);
        }

        [HttpPut("{productId}/images/{imageId}")]
        [Authorize]
        public async Task<IActionResult> UpdateImage(int imageId, [FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _ProductService.UpdateImage(imageId, request);
            if (result == 0)
                return BadRequest();//400

            return Ok();
        }

        [HttpDelete("{productId}/images/{imageId}")]
        [Authorize]
        public async Task<IActionResult> RemoveImage(int imageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _ProductService.RemoveImage(imageId);
            if (result == 0)
                return BadRequest();//400

            return Ok();
        }

        [HttpGet("{productId}/images/{imageId}")]
        public async Task<IActionResult> GetImageById(int productId, int imageId)
        {
            var image = await _ProductService.GetImageById(imageId);
            if (image == null)
                return BadRequest("Không tim thấy hình ảnh");
            return Ok(image);
        }

        [HttpPut("{id}/categories")]
        [Authorize]
        public async Task<IActionResult> CategoryAssign(int id, [FromBody] CategoryAssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _ProductService.CategoryAssign(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("featured/{languageId}/{take}")]
        public async Task<IActionResult> GetFeaturedProducts(string languageId, int take)
        {
            var products = await _ProductService.GetFeatureProducts(languageId, take);

            return Ok(products);
        }

        [HttpGet("latest/{languageId}/{take}")]
        public async Task<IActionResult> GetLatestProducts(string languageId, int take)
        {
            var products = await _ProductService.GetLatestProducts(languageId, take);

            return Ok(products);
        }

        [HttpGet("related/{languageId}/{take}")]
        public async Task<IActionResult> GetRelatedProducts(string languageId, int take)
        {
            var products = await _ProductService.GetRelatedProducts(languageId, take);

            return Ok(products);
        }

        #region Lập Trình Tiên Tiến. Lịch Công Tác

        [HttpPost("CreateWs")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateWS([FromForm] WorkingscheduleViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var workingscheduleId = await _ProductService.CreateWS(request);
            if (workingscheduleId == 0)
                return BadRequest();//400

            var workingschedule = await _ProductService.GetByIdWS(workingscheduleId);

            return CreatedAtAction(nameof(GetById), new { id = workingscheduleId }, workingschedule);
        }

        [HttpPut("UpdateWs/{workingscheduleId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateWS([FromRoute] int workingscheduleId, [FromForm] WorkingscheduleViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            request.Id = workingscheduleId;
            var affectedResult = await _ProductService.UpdateWS(request);
            if (affectedResult == 0)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("Delete/{workingId}")]
        public async Task<IActionResult> DeleteWorkingSchedule(int workingId)
        {
            var affectedResult = await _ProductService.DeleteWS(workingId);
            if (affectedResult == 0)
                return BadRequest();

            return Ok();
        }

        [HttpGet("FindId/{workingId}")]
        public async Task<IActionResult> GetByIdWS(int workingId)
        {
            var workingschedule = await _ProductService.GetByIdWS(workingId);
            if (workingschedule == null)
                return BadRequest("Không tim thấy lịch công tác");
            return Ok(workingschedule);
        }

        [HttpGet("listWorkingSchedule")]
        public async Task<IActionResult> GetAll([FromQuery] GetUserPagingRequest request)
        {
            var workingschedule = await _ProductService.GetAllPagingWS(request);
            return Ok(workingschedule);
        }

        #endregion Lập Trình Tiên Tiến. Lịch Công Tác
    }
}