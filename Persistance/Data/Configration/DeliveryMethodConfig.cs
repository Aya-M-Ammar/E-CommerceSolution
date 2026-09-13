using E_Commerce.Domain.Entity.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistance.Data.Configration
{
    public class DeliveryMethodConfig : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(P => P.Price).HasPrecision(8, 2);

            builder.Property(P => P.Description).HasMaxLength(100);
            builder.Property(P => P.DeliveryTime).HasMaxLength(50);
            builder.Property(P => P.ShortName).HasMaxLength(50);


        }
    }
}
