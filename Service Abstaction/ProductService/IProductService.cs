
using Shared.DTOS.ProductDTOS;
using Shared.Result;

namespace Service_Abstaction.ProductService
{
    public interface IProductService
    {
        Task<PaginatedResult<ProductDTO>> GetAllProductsAsync( ProductQueryParams QuerParams);

        Task<Result<ProductDTO>> GetProductByIdAsync(int id);
        Task<IEnumerable<BrandDTO>> GetAllBrandsAsync();
        Task<IEnumerable<TypeDTO>> GetAllTypesAsync();

    }
}