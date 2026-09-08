using E_Commerce.Domain.Entity.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistance.Data.Configration
{
    public class ProductConfigration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(X => X.Name).HasMaxLength(100);
            builder.Property(X=>X.Description).HasMaxLength(500);
            builder.Property(X => X.PictureURL).HasMaxLength(200);
            builder.Property(X => X.Price).HasPrecision(18, 2);
            builder.HasOne(X=>X.ProductBrands).WithMany().HasForeignKey(x => x.BrandId);
            builder.HasOne(X => X.ProductType).WithMany().HasForeignKey(x => x.TypeId);

            
    }
    }
}
