using Talabat.APIs.Extensions;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities.Identity;
using Talabat.Infrastructure._Identity;
using Talabat.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Talabat.APIs 
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args); 

            #region Configure Services

            builder.Services.AddControllers();

            builder.Services.AddSwaggerServices();

            builder.Services.AddApplicationServices();

            builder.Services.AddDatabasesConnection(builder.Configuration);

            builder.Services.AddAuthServices(builder.Configuration);

            builder.Services.AddPolicesConfigurations(builder.Configuration);

            #endregion

            var app = builder.Build();

            #region Data Seedings and Migrations
            // Add Any not applied Migrations to DB
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var _dbContext = services.GetRequiredService<StoreContext>();
            var _identityDbContext = services.GetRequiredService<ApplicationIdentityDbContext>();
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                await _dbContext.Database.MigrateAsync();
                await StoreContextSeed.SeedAsync(_dbContext);
                await _identityDbContext.Database.MigrateAsync();
                var _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                await ApplicationIdentityDbContextSeed.SeedUsersAsync(_userManager);

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error has been occured during Migration");
            }

            #endregion

            #region Configure Kestrel Middlewares

            app.UseMiddleware<ExceptionMiddleware>(); // Custom Exception Middleware

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) app.UseSwaggerMiddlewares();

            app.UseStatusCodePagesWithReExecute("/errors/{0}"); // Custom Error Handling Middleware

            //app.UseHttpsRedirection(); // Redirect HTTP to HTTPS

            app.UseStaticFiles(); // Serve Static Files

            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers(); // Map Controllers to the Application

            #endregion

            app.Run();
        }
    }
}
