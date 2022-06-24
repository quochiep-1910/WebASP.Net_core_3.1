namespace eShop.Utilities.Constants
{
    public class SystemConstants
    {
        public const string MainConnecttionString = "eShopDb";
        public const string CartSession = "CartSession";

        public class AppSettings
        {
            public const string DefaultLanguageId = "DefaultLanguageId";
            public const string Token = "Token";

            public const string BaseAddress = "BaseAddress";
        }

        public class ProductSettings
        {
            public const int NumberOfFeatureProducts = 6;
            public const int NumberOfLastestProducts = 3;
            public const int NumberOfRelatedProducts = 3;
        }

        public class ProductConstants
        {
            public const string NA = "N/A";
        }

        public class CategoryConstants
        {
            public const string NA = "N/A";
        }

        public enum Status
        {
            InActive,
            Active
        }

        public enum OrderStatus
        {
            InProgress,
            Confirmed,
            Shipping,
            Success,
            Canceled
        }

        public class OrderConstants
        {
            public const string CannotFindOrderCode = "Không tim thấy mã hoá đơn";
            public const string OrderSuccess = "Thành công";
            public const string OrderSaveInDataAndSuccess = "Đơn hàng đã được ghi nhận và thanh toán thành công";
            public const string OrderIsCancel = "Đơn hàng đã bị huỷ bỏ";
            public const string OrderIsRecodeInData = "Đơn hàng đã được ghi nhận";
            public const string OrderIsFail = "Đơn đặt hàng thất bại";
        }

        public class UserTokenConstants
        {
            public const string AuthenticatorKey = "AuthenticatorKey";
            public const string AspNetUserStore = "[AspNetUserStore]";
            public const string RecoveryCodes = "RecoveryCodes";
        }
    }
}