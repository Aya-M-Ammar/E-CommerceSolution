using AutoMapper;
using E_Commerce.Domain.Entity.Order;
using Microsoft.Extensions.Configuration;
using Shared.DTOS.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Implementation.Mapping
{
    public class OrderPictureURLValueResolver : IValueResolver<OrderItem, OrderItemDTO, string>
    {
        private readonly IConfiguration _configration;

        public OrderPictureURLValueResolver(IConfiguration configration)
        {
            _configration = configration;
        }
        public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.Product.PictureURL))
            {
                return string.Empty;
            }
            if (source.Product.PictureURL.StartsWith("http"))
            {
                return source.Product.PictureURL;
            }
            else
            {
                var BaseUrl = _configration.GetSection("URLs:BaseURL").Value;
                if (string.IsNullOrEmpty(BaseUrl)) return string.Empty;
                return $"{BaseUrl}/{source.Product.PictureURL}";
            }
        }
    }
}