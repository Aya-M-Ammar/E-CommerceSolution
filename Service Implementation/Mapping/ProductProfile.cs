using AutoMapper;
using E_Commerce.Domain.Entity.Basket;
using E_Commerce.Domain.Entity.BasketModel;
using E_Commerce.Domain.Entity.Product;
using Shared.DTOS.BasketDTOs;
using Shared.DTOS.ProductDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Service_Implementation.Mapping
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(des=>des.ProductBrand,src=>src.MapFrom(s=>s.ProductBrands.Name))
                .ForMember(des=>des.ProductType,src=>src.MapFrom(s=>s.ProductType.Name))
               .ForMember(des=>des.PictureUrl,src=>src.MapFrom<ProductPictureResolver>());

            CreateMap<ProductBrand, BrandDTO>().ReverseMap();
            CreateMap<ProductType, TypeDTO>().ReverseMap();
          
        }
    }
}

