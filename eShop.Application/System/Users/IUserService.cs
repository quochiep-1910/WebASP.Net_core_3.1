using eShop.ViewModels.Common;
using eShop.ViewModels.System;
using eShop.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.System.Users
{
    public interface IUserService
    {
        Task<ApiResult<string>> Authencate(LoginRequest loginRequest);

        Task<ApiResult<bool>> Register(RegisterRequest registerRequest);

        Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest registerRequest);

        Task<ApiResult<PagedResult<UserViewModel>>> GetUserPaging(GetUserPagingRequest request);

        Task<ApiResult<UserViewModel>> GetById(Guid id);

        Task<ApiResult<bool>> Delete(Guid id);

        Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request);
    }
}