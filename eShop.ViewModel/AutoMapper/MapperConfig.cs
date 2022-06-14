using AutoMapper;
using eShop.Data.Entities;
using eShop.ViewModels.Sales.Order;
using eShop.ViewModels.Sales.OrderDetail;

namespace eShop.ViewModels.AutoMapper
{
    public class MapperConfig : Profile
    {
        //Config AutoMapper
        public MapperConfig()
        {
            //CreateMap<AppUser, AppUserBasicDTO>().ReverseMap();
            CreateMap<Order, OrderViewModel>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailTimeLineViewModel>().ReverseMap();
        }
    }
}