namespace eShop.Utilities.Constants
{
    public static class ResponseMessage
    {
        public const string AddFailure = "Failed To Add New Resource";
        public const string DeleteFailure = "Failed To Delete New Resource";
        public const string UpdateFailure = "Failed To Update New Resource";
        public const string AddSuccess = "Add New Resource Successfully";
        public const string UpdateSuccess = "Update Requested Resource Successfully";
        public const string DeleteSuccess = "Delete Requested Resource Successfully";
        public const string LoginFailure = "Invalid login request";
        public const string IncorrectPassword = "Password is incorrect";
        public const string RevokedToken = "Token Has Been Revoked";
        public const string EmailExist = "Email already in use";
        public const string RegisterFailure = "Invalid register request";
        public const string ACCESS_DENIED = "Access Denied";
        public const string NOT_MATCH = "Id doesn't match";

        public static string OBJECT_EXIST(string email) => $"{email} exist in system";

        public static string RESOURCE_NOTFOUND(string id) => $"{id} Not Found";

        public const string GetDataSuccess = "Get Data Successfully";
        public const string GetDataFailed = "Get Data Failed";
        public static string IN_DISCOUNT = "Course in sales off date";
        public static string ExistCartItem = "Item is already in your cart";
        public const string CouponUse = "You use this coupon before";
        public const string EndDate = "Coupon is finished";
        public const string AddMediaWithoutCourse = "Media must be create with new course";
        public const string TitleMaxLengthInvalid = "Title max length is 100";
        public const string ShortDescriptionMaxLengthInvalid = "Short description max length is 220";
        public const string AddPriceWithoutCourse = "Price must be create with new course";

        public static string NotMatch { get; set; }
    }
}