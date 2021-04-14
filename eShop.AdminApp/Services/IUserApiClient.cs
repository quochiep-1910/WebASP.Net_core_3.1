using eShop.ViewModels.Common;
using eShop.ViewModels.System;
using eShop.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.AdminApp.Services
{
    public interface IUserApiClient
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);

        Task<ApiResult<PagedResult<UserViewModel>>> GetUsersPagings(GetUserPagingRequest request);

        Task<ApiResult<bool>> RegisterUser(RegisterRequest registerRequestrequest);

        Task<ApiResult<bool>> UpdateUser(Guid id, UserUpdateRequest userUpdate);

        Task<ApiResult<UserViewModel>> GetById(Guid id);
    }
}