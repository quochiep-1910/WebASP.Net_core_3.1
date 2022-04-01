using eShop.ViewModels.Common;
using System.Collections.Generic;

namespace eShop.ViewModels.System.Users
{
    public class RoleAssignRequest
    {
        public string id { get; set; }
        public List<SelectItem> Roles { set; get; } = new List<SelectItem>(); //gán giá trị mặc định tránh null
    }
}