using eShop.Application.Sales;
using eShop.Data.EF;
using eShop.ViewModels.Sales.Order;
using eShop.ViewModels.Sales.RevenueStatistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShop.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly EShopDbContext _context;
        private readonly IOrderService _OrderService;

        public OrdersController(IOrderService OrderService, EShopDbContext context)
        {
            _OrderService = OrderService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var orderId = await _OrderService.Create(request);
            if (orderId == 0)
                return BadRequest();//400
            var order = await _OrderService.GetById(orderId);

            return CreatedAtAction(nameof(GetById), new { id = orderId }, order);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetById(int orderId)
        {
            var order = await _OrderService.GetById(orderId);
            if (order == null)
                return BadRequest("Không tim thấy mã hoá đơn");
            return Ok(order);
        }

        [HttpDelete("{orderId}")]
        [Authorize]
        public async Task<IActionResult> Delete(int orderId)
        {
            var affectedResult = await _OrderService.Delete(orderId);
            if (affectedResult == 0)
                return BadRequest();

            return Ok();
        }

        [HttpGet("paging")]
        [Authorize]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetOrderPagingRequest request)
        {
            var order = await _OrderService.GetOrderPaging(request);

            return Ok(order);
        }

        [HttpPut("{orderId}")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int orderId, [FromForm] OrderUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            request.Id = orderId;
            var affectedResult = await _OrderService.Update(request);
            if (affectedResult == 0)
                return BadRequest();

            return Ok();
        }

        [Route("getrevenue")]
        [HttpGet]
        public async Task<IActionResult> GetRevenuesStatistic([FromQuery] StatisticsRequest request)
        {
            var affectedResult = await _OrderService.GetRevenueStatistic(request);

            return Ok(affectedResult);
        }
    }
}