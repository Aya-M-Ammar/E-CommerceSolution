using AutoMapper;
using E_Commerce.Domain.Entity.Product;
using E_Commerce.Domain.Interfaces.Repository;
using Service_Abstaction.ProductService;
using Service_Implementation.Exciptions;
using Service_Implementation.Specification;
using Shared.DTOS.ProductDTOS;
using Shared.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Implementation.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IUniteOfWork _unitofwork;
        private readonly IMapper _mapper;

        public ProductService(IUniteOfWork unitofwork, IMapper mapper)
        {
            _unitofwork = unitofwork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<BrandDTO>> GetAllBrandsAsync()
        {
            var Brands = await _unitofwork.GetRepositoryAsync<ProductBrand, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<BrandDTO>>(Brands);

        }

        public async Task<PaginatedResult<ProductDTO>> GetAllProductsAsync(ProductQueryParams QuerParams)
        {
            ProductWithBrandAndTypeSpecification spc = new ProductWithBrandAndTypeSpecification(QuerParams);

            var Products = await _unitofwork.GetRepositoryAsync<Product, int>().GetAllAsync(spc);

            var DataToReturn = _mapper.Map<IEnumerable<ProductDTO>>(Products);
            var CountOfReturnData = DataToReturn.Count();

            return new PaginatedResult<ProductDTO>(QuerParams.PageIndex, CountOfReturnData, CountOfReturnData, DataToReturn);
        }

        public async Task<IEnumerable<TypeDTO>> GetAllTypesAsync()
        {
            var Types = await _unitofwork.GetRepositoryAsync<ProductType, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<TypeDTO>>(Types);
        }

        public async Task<Result<ProductDTO>> GetProductByIdAsync(int id)
        {
            var spce = new ProductWithBrandAndTypeSpecification(id);
            var Product = await _unitofwork.GetRepositoryAsync<Product, int>().GetByIdAsync(spce);
            if (Product is null)
                return  Error.NotFound($"Product{id}NotFound", "Product.NotFound");
            return _mapper.Map<ProductDTO>(Product);
        }
    }
}
