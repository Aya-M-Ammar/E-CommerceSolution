using AutoMapper;
using E_Commerce.Domain.Entity.Basket;
using E_Commerce.Domain.Entity.BasketModel;
using Shared.DTOS.BasketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Implementation.Mapping
{
    public class BasketProfile:Profile
    {
        public BasketProfile()
        {
            CreateMap<BasketCustomer, BasketDTO>().ReverseMap();
            CreateMap<BasketItem, BasketItemDTO>().ReverseMap();
        }
    }
}
