using Talabat.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Talabat.Core.Services.Contract
{
    public interface IAuthService
    {
        Task<string> CreateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager);
    }
}
