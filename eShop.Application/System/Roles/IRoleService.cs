using eShop.ViewModels.System.Roles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.Application.System.Roles
{
    public interface IRoleService
    {
        Task<List<RoleViewModel>> GetAll();
    }
}