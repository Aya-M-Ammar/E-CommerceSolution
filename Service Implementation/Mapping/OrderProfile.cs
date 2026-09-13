using AutoMapper;
using E_Commerce.Domain.Entity.Order;
using Shared.DTOS.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Implementation.Mapping
{
    public class OrderProfile:Profile
    {
        public OrderProfile()
        {
            CreateMap<AddsressDTO,OrderAddress>().ReverseMap();
            CreateMap<Order,OrderToReturnDTO>()
                .ForMember(D=>D.DleviryMethod,O=>O.MapFrom(src=>src.DeliveryMethod.ShortName)).ReverseMap();
            CreateMap<OrderItem,OrderItemDTO>().ForMember(D => D.ProductName,
            O => O.MapFrom(src => src.Product.ProductName))
              . ForMember(D => D.PicturewURL,
            O => O.MapFrom(src => src.Product.PictureURL))
            .ReverseMap();
                
        }
    }
}
