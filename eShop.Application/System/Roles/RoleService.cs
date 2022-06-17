using eShop.Data.EF;
using eShop.ViewModels.System.Roles;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.Application.System.Roles
{
    public class RoleService : IRoleService
    {
        private readonly EShopDbContext _context;

        public RoleService(EShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoleViewModel>> GetAll()
        {
            var roles = await _context.Roles.Select(x => new RoleViewModel()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync();
            return roles;
        }
    }
}