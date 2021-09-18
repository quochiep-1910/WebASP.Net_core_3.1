using eShop.Application.Sales;
using eShop.ViewModels.Sales.Order;
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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _OrderService;

        public OrdersController(IOrderService OrderService)
        {
            _OrderService = OrderService;
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
    }
}