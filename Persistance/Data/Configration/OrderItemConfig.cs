using E_Commerce.Domain.Entity.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistance.Data.Configration
{
    public class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(P => P.Price).HasPrecision(8, 2);
            builder.OwnsOne(X => X.Product, OEntity =>
            {
                OEntity.Property(P => P.PictureURL).HasMaxLength(50);
                OEntity.Property(P => P.ProductName).HasMaxLength(50);
              


            });
        }
    }
}
