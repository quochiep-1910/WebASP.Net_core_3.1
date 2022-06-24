using AspNetCoreHero.ToastNotification.Abstractions;
using eShop.ApiIntegration;
using eShop.Utilities.Constants;
using eShop.ViewModels.Sales.Order;
using eShop.ViewModels.Sales.OrderDetail;
using eShop.WebApp.Models;
using eShop.WebApp.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
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

        public async Task<IActionResult> Checkout()
        {
            if (string.IsNullOrEmpty(Request.QueryString.ToString()))
            {
                return View(GetCheckoutViewModel());
            }
            var result = NotifyUrl();

            _notyf.Information(result.Result.ToString());
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

            #region
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
                ModelState.AddModelError("", SystemConstants.OrderConstants.OrderIsFail);
                return View(request);
            }

            var momoPay = PaymentMomo(result);
            if (momoPay.Result.GetValue("errorCode").ToString() != "0")
            {
                _notyf.Error(momoPay.Result.GetValue("localMessage").ToString());
                return RedirectToAction("Checkout");
            }
            else
            {
                return Redirect(momoPay.Result.GetValue("payUrl").ToString());
            }

            ////remove session cart
            //HttpContext.Session.Remove(SystemConstants.CartSession);
            //_notyf.Information("Đơn đặt hàng thành công");
            //TempData["SuccessMsg"] = "Đơn đặt hàng thành công";
            #endregion
            //return View(model);
        }

        #region Momo Paymemt

        private async Task<JObject> PaymentMomo(int orderId)
        {
            var model = GetCheckoutViewModel();
            string totalPrice = "";

            totalPrice += model.CartItems.Sum(x => x.Quantity * x.Price).ToString("0.##");

            //request params need to request to MoMo system
            string endpoint = "https://payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOPHOV20220618";
            string accessKey = "Oo5q0jrT0XuMrwPk";
            string serectkey = "Dr9jRAbvXg4mhOnawa7K4dXEfj8PCWod";
            string orderInfo = orderId.ToString();
            string returnUrl = "https://localhost:44308/vi-VN/cart/checkout";
            string notifyurl = "https://localhost:44308/vi-VN/cart/checkout"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

            string amount = totalPrice;
            string orderid = DateTime.Now.Ticks.ToString();
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
                {
                    { "partnerCode", partnerCode },
                    { "accessKey", accessKey },
                    { "requestId", requestId },
                    { "amount", amount },
                    { "orderId", orderid },
                    { "orderInfo", orderInfo },
                    { "returnUrl", returnUrl },
                    { "notifyUrl", notifyurl },
                    { "extraData", extraData },
                    { "requestType", "captureMoMoWallet" },
                    { "signature", signature }
                };

            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);
            return jmessage;
            //return Redirect(jmessage.GetValue("payUrl").ToString());
        }

        private string ReturnUrl()
        {
            string param = Request.QueryString.ToString().Substring(0, Request.QueryString.ToString().IndexOf("signature") - 1);

            param = UrlEncoder.Default.Encode(param);
            MoMoSecurity cryto = new MoMoSecurity();
            string serectkey = "Dr9jRAbvXg4mhOnawa7K4dXEfj8PCWod";
            string signature = cryto.signSHA256(param, serectkey);
            if (signature != Request.Query["signature"].ToString())
            {
                ViewBag.message = "Thông tin Request không hợp lệ";
                return "Thông tin Request không hợp lệ";
            }
            if (!Request.Query["errorCode"].Equals("0"))
            {
                return "Thanh toán thất bại";
            }
            else
            {
                return "Thanh toán thành công";
            }
        }

        public async Task<string> NotifyUrl()
        {
            string param = Request.QueryString.ToString().Substring(0, Request.QueryString.ToString().IndexOf("signature") - 1);
            param = UrlEncoder.Default.Encode(param);
            MoMoSecurity crypto = new MoMoSecurity();
            string serectkey = "Dr9jRAbvXg4mhOnawa7K4dXEfj8PCWod";
            string signature = crypto.signSHA256(param, serectkey);
            if (signature != Request.Query["signature"].ToString())
            {
            }
            string status_code = Request.Query["errorCode"].ToString();
            if (status_code == "0")
            {
                //Success - update status order

                var order = new ChangeStatusOrder()
                {
                    OrderId = Int32.Parse(Request.Query["orderInfo"].ToString()),
                    Status = 3 //success
                };
                await _orderApiClient.ChangeStatusOrder(order);
                //remove session cart
                HttpContext.Session.Remove(SystemConstants.CartSession);
                return SystemConstants.OrderConstants.OrderSaveInDataAndSuccess;
            }
            else if (status_code == "49")
            {
                //failure - Order Is Cancel

                var order = new ChangeStatusOrder()
                {
                    OrderId = Int32.Parse(Request.Query["orderInfo"].ToString()),
                    Status = 4 //cancel
                };
                await _orderApiClient.ChangeStatusOrder(order);
                return SystemConstants.OrderConstants.OrderIsCancel;
            }
            else
            {
                //remove session cart
                HttpContext.Session.Remove(SystemConstants.CartSession);
                return SystemConstants.OrderConstants.OrderIsRecodeInData;
            }
            //return Request.Query["localMessage"].ToString();
        }

        #endregion Momo Paymemt

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