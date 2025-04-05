using Talabat.Infrastructure._Identity;
using Talabat.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Talabat.APIs.Extensions
{
    public static class DatabaseConnectionServiceExtension
    {
        public static IServiceCollection AddDatabasesConnection(this IServiceCollection Services, IConfiguration config)
        {
            Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) => {
                var connection = config.GetConnectionString("redis");
                return ConnectionMultiplexer.Connect(connection);
            });

            Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            Services.AddDbContext<ApplicationIdentityDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("IdentityConnection"));
            });
            return Services;
        }
    }
}
