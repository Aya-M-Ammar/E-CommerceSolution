using AutoMapper;
using E_Commerce.Domain.Entity.Product;
using Microsoft.Extensions.Configuration;
using Shared.DTOS.ProductDTOS;

namespace Service_Implementation.Mapping
{
    public class ProductPictureResolver : IValueResolver<Product, ProductDTO, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(
            Product source,
            ProductDTO destination,
            string destMember,
            ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.PictureURL))
                return string.Empty;

            if (source.PictureURL.StartsWith("http"))
                return source.PictureURL;

            var baseUrl = _configuration.GetSection("URLs")["BaseURL"];

            if (string.IsNullOrEmpty(baseUrl))
                return string.Empty;

            return $"{baseUrl.TrimEnd('/')}/{source.PictureURL}";
        }
    }
}