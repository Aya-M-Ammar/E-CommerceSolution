using E_Commerce.Presentation.Attributes;
using Microsoft.AspNetCore.Mvc;
using Service_Abstaction.ProductService;
using Shared.DTOS.ProductDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controller
{

 
  
    public class ProductController : ApiBaseController
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }
        [HttpGet]
        [RedisCache]
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProduct([FromQuery] ProductQueryParams QuerParams)
        {
            var product = await _service.GetAllProductsAsync(QuerParams);
            return Ok(product);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var product = await _service.GetProductByIdAsync(id);
            return HandleResult<ProductDTO>(product);
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllBrand()
        {
            var brands = await _service.GetAllBrandsAsync();
            return Ok(brands);
        }
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<TypeDTO>>> GetAllType()
        {
            var Types = await _service.GetAllTypesAsync();
            return Ok(Types);
        }





    }
}