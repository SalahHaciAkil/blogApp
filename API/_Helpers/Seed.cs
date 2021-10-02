using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API._Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API._Helpers
{
    public class Seed
    {
        public static async Task SeedDataAsync(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {

            if (await userManager.Users.AnyAsync()) return;


            var roles = new List<AppRole>{
                new AppRole{Name = "Admin"},
                new AppRole{Name = "Member"}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            var admin = new AppUser
            {
                UserName = "admin",
                Email = "salahaljable@gmail.com"
            };

            await userManager.CreateAsync(admin, "E3we2e3we2");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Member" });


        }
    }
}