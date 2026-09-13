using E_Commerce.Domain.Entity.IDentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistance.IdentityData.IDentityContext
{
    public class StoreIdentityContext:IdentityDbContext
    {
        public StoreIdentityContext(DbContextOptions<StoreIdentityContext> options):base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
           base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<Address>().ToTable("Addresses");
            builder.Entity<ApplicationUser>().HasOne(a => a.Address).WithOne(a => a.User).HasForeignKey<Address>(a => a.UserId);
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        }
    }
}
