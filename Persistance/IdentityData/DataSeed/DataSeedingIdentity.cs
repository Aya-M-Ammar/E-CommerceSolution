using E_Commerce.Domain.Entity.IDentity;
using E_Commerce.Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistance.IdentityData.DataSeed
{
    public class DataSeedingIdentity : IDataInitialize
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<DataSeedingIdentity> _logger;

        public DataSeedingIdentity(UserManager<ApplicationUser> userManager
            , RoleManager<IdentityRole> roleManager
            ,ILogger<DataSeedingIdentity> logger)
        {
            this._userManager = userManager;
          
            this._roleManager = roleManager;
            _logger = logger;
        }
        public async Task InitializeASync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                   await _roleManager.CreateAsync(new IdentityRole("Admin"));
                  await  _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }
                if (!_userManager.Users.Any())
                {
                    ApplicationUser user1 = new ApplicationUser()
                    {
                        UserName = "AyaAmmar",
                        Email = "ayaammar22@gmail.com",
                        DisplayName = "Aya Ammar",
                        PhoneNumber= "01065333256"
                    };
                    ApplicationUser user2 = new ApplicationUser()
                    {
                        UserName = "MahmoudAmmar",
                        Email = "mahmoudammar22@gmail.com",
                        DisplayName = "Mahmoud Ammar",
                        PhoneNumber = "01065333776",
                        
                    };
                  await  _userManager.CreateAsync(user1, "Admin@123");
                  await  _userManager.CreateAsync(user2, "Admin@123");

                    await _userManager.AddToRoleAsync(user1, "Admin");
                    await _userManager.AddToRoleAsync(user2, "SuperAdmin");

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Data Seeding: {ex.Message}");
            }
        }
    }
}
