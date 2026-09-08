using E_Commerce.Domain.Entity.Product;
using Service_Abstaction.Specification;
using Shared.DTOS.ProductDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Implementation.Specification
{
    internal class ProductWithBrandAndTypeSpecification: BaseSpacification<Product,int>
    {
        public ProductWithBrandAndTypeSpecification(int id) : base(P => P.Id == id)
        {

            AddInclude(P => P.ProductType);
            AddInclude(P => P.ProductBrands);
        }
        public ProductWithBrandAndTypeSpecification(ProductQueryParams QuerParams) :
            base(P => (!QuerParams.BrandId.HasValue || P.BrandId == QuerParams.BrandId.Value) &&
            (!QuerParams.TypeId.HasValue || P.TypeId == QuerParams.TypeId.Value)
            && (string.IsNullOrEmpty(QuerParams.Search) || P.Name.ToLower().Contains(QuerParams.Search.ToLower())))
        {

            AddInclude(P => P.ProductType);
            AddInclude(P => P.ProductBrands);
            switch (QuerParams.Sort)
            {
                case Sort.NameAsc:
                    Addorder(P => P.Name);
                    break;
                case Sort.NameDesc:
                    AddorderDescinding(P => P.Name);
                    break;
                case Sort.PriceDesc:
                    AddorderDescinding(P => P.Price);
                    break;
                case Sort.PriceAsc:
                    Addorder(P => P.Price);
                    break;
                default:
                    Addorder(P => P.Id);
                    break;


            }


        }
    }
}
