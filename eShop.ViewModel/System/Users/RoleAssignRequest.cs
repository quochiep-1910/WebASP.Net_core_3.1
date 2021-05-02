using eShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.ViewModels.System.Users
{
    public class RoleAssignRequest
    {
        public Guid id { get; set; }
        public List<SelectItem> Roles { set; get; } = new List<SelectItem>(); //gán giá trị mặc định tránh null
    }
}