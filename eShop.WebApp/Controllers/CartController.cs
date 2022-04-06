using AspNetCoreHero.ToastNotification.Abstractions;
using eShop.ApiIntegration;
using eShop.Utilities.Constants;
using eShop.ViewModels.Sales.Order;
using eShop.ViewModels.Sales.OrderDetail;
using eShop.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IOrderApiClient _orderApiClient;
        private IUserApiClient _userApiClient;
        private readonly INotyfService _notyf;

        public CartController(IProductApiClient productApiClient, INotyfService notyf, IOrderApiClient orderApiClient
, IUserApiClient userApiClient)
        {
            _productApiClient = productApiClient;
            _notyf = notyf;
            _orderApiClient = orderApiClient;
            _userApiClient = userApiClient;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var total = 0;
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
            {
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
                foreach (var countQuantity in currentCart)
                {
                    total += countQuantity.Quantity;
                }
                _notyf.Information($"Có {total} trong giỏ hàng");
            }

            return View(currentCart);
        }

        public async Task<IActionResult> AddToCart(int id, string languageId)
        {
            var product = await _productApiClient.GetById(id, languageId);

            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
            {
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
                int quantity = 1;
                var list = (List<CartItemViewModel>)currentCart;

                if (list.Exists(x => x.ProductId == id))
                {
                    foreach (var item in list)
                    {
                        if (item.ProductId == id)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    var cartItem = new CartItemViewModel()
                    {
                        ProductId = id,
                        Description = product.Description,
                        Image = product.ThumbnailImage,
                        Name = product.Name,
                        Quantity = quantity,
                        Price = product.Price
                    };
                    list.Add(cartItem);
                }

                HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(list));
            }
            else
            {
                int quantity = 1;
                if (currentCart.Any(x => x.ProductId == id))
                {
                    quantity = currentCart.First(x => x.ProductId == id).Quantity + 1;
                }

                var cartItem = new CartItemViewModel()
                {
                    ProductId = id,
                    Description = product.Description,
                    Image = product.ThumbnailImage,
                    Name = product.Name,
                    Quantity = quantity,
                    Price = product.Price
                };

                currentCart.Add(cartItem);
            }

            HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));

            return Ok(currentCart);
        }

        public IActionResult UpdateCart(int id, int quantity)
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);

            foreach (var item in currentCart)
            {
                if (item.ProductId == id)
                {
                    if (quantity == 0)
                    {
                        currentCart.Remove(item);
                        break;
                    }
                    item.Quantity = quantity;
                }
            }

            HttpContext.Session.SetString(SystemConstants.CartSession, JsonConvert.SerializeObject(currentCart));

            return Ok(currentCart);
        }

        public IActionResult Checkout()
        {
            return View(GetCheckoutViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutViewModel request)
        {
            var model = GetCheckoutViewModel();

            var username = User.Identity.Name;
            if (username == request.CheckoutModel.ShipName)
            {
                request.CheckoutModel.ShipName = username;
            }
            var orderDetails = new List<OrderDetailViewModel>();
            foreach (var item in model.CartItems)
            {
                orderDetails.Add(new OrderDetailViewModel()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }
            var checkoutRequest = new OrderCreateRequest()
            {
                ShipAddress = request.CheckoutModel.ShipAddress,
                ShipName = request.CheckoutModel.ShipName,
                ShipEmail = request.CheckoutModel.ShipEmail,
                ShipPhoneNumber = request.CheckoutModel.ShipPhoneNumber,
                OrderDetails = orderDetails,
                Status = request.CheckoutModel.Status,
                UserId = request.CheckoutModel.UserId,
                OrderDate = DateTime.Now
            };

            //TODO: Add to API

            var result = await _orderApiClient.CreateOrder(checkoutRequest);
            if (result == 0)
            {
                ModelState.AddModelError("", "Đơn đặt hàng thất bại");
                return View();
            }
            _notyf.Information("Đơn đặt hàng thành công");
            TempData["SuccessMsg"] = "Đơn đặt hàng thành công";
            return View(model);
        }

        private CheckoutViewModel GetCheckoutViewModel()
        {
            var session = HttpContext.Session.GetString(SystemConstants.CartSession);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();

            if (session != null)
            {
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            }

            var checkoutVm = new CheckoutViewModel()
            {
                CartItems = currentCart,
                CheckoutModel = new OrderCreateRequest()
            };
            return checkoutVm;
        }

        //lấy thông tin đăng nhập
        public async Task<IActionResult> GetUser()
        {
            if (User.Identity.IsAuthenticated)//đã đăng nhập
            {
                var username = User.Identity.Name;
                var user = await _userApiClient.GetByUserName(username);
                return Ok(user);
            }
            return BadRequest("Không lấy được thông tin");
        }
    }
}