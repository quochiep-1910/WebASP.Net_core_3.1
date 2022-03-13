using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace eShop.Utilities.Exceptions
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        ///Lọc lỗi thông báo từ server hiện lên trên màn hình hình
        public void OnException(ExceptionContext context)
        {
            _logger.LogError($"Caught in ExceptionFilter {context.Exception.Message}");

            var result = new JsonResult("Something went wrong");
            result.StatusCode = 500;

            context.Result = result;
        }
    }
}