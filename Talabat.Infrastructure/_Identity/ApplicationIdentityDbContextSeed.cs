using Talabat.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Talabat.Infrastructure._Identity
{
    public static class ApplicationIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new ApplicationUser()
                {
                    DisplayName = "Kareem Wardany",
                    Email = "kareem.m.wardany@gmail.com",
                    UserName = "kareem.wardany",
                    PhoneNumber = "01144106127"
                };
                await userManager.CreateAsync(user, "P@ssw0rd");
            }
        }
    }
}
