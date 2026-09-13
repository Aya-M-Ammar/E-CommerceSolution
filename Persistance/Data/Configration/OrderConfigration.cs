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
    public class OrderConfigration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(P => P.SubTotal).HasPrecision(8, 2);
            builder.OwnsOne(X => X.Address, OEntity =>
            {
                OEntity.Property(P => P.FName).HasMaxLength(50);
                OEntity.Property(P => P.LName).HasMaxLength(50);
                OEntity.Property(P => P.Street).HasMaxLength(50);
                OEntity.Property(P => P.Country).HasMaxLength(50);
                OEntity.Property(P => P.City).HasMaxLength(50);


            });
        }
    }
}
