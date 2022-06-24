using eShop.Application.Sales;
using eShop.Application.System.Users;
using eShop.Data.EF;
using eShop.Utilities.Constants;
using eShop.ViewModels.Sales.Order;
using eShop.ViewModels.Sales.OrderDetail;
using eShop.ViewModels.Sales.RevenueStatistics;
using eShop.ViewModels.Utilities.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly EShopDbContext _context;
        private readonly IOrderService _OrderService;
        private readonly IUserService _userService;

        public OrdersController(IOrderService OrderService, EShopDbContext context, IUserService userService)
        {
            _OrderService = OrderService;
            _context = context;
            _userService = userService;
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

            return Ok(orderId);
        }

        [HttpPost("changeStatusOrder")]
        public async Task<IActionResult> ChangeStatusOrder([FromBody] ChangeStatusOrder order)
        {
            var updateOrder = await _OrderService.ChangeStatusOrder(order.OrderId, order.Status);
            return Ok(updateOrder);
        }

        [HttpPost("postorderdetail")]
        public async Task<IActionResult> CreateOrderDetail([FromBody] List<OrderDetailViewModel> request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _OrderService.CreateOrderDetail(request);
            if (result == 0)
                return BadRequest();//400
            var order = await _OrderService.GetById(request.Select(x => x.OrderId).FirstOrDefault());

            return CreatedAtAction(nameof(GetById), new { id = request.Select(x => x.OrderId).FirstOrDefault() }, order);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetById(int orderId)
        {
            var order = await _OrderService.GetById(orderId);
            if (order == null)
                return BadRequest(SystemConstants.OrderConstants.CannotFindOrderCode);
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

        [HttpGet("totalOrder")]
        [Authorize]
        public async Task<IActionResult> GetTotalOrder()
        {
            var totalOrder = await _OrderService.GetTotalOrder();

            return Ok(totalOrder);
        }

        [HttpGet("GetTotalOrderById")]
        [Authorize]
        public async Task<IActionResult> GetTotalOrderById(string id)
        {
            var totalOrderById = await _OrderService.GetTotalOrderById(id);

            return Ok(totalOrderById);
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

        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail([FromForm] SendMailViewModel sendMailViewModel)
        {
            var sendEmailRequest = await _userService.SendEmailRequest(sendMailViewModel);
            return Ok();
        }
    }
}