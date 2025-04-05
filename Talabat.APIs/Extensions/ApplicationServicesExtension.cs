using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Application.CacheService;
using Talabat.Application.OrderService;
using Talabat.Application.PaymentService;
using Talabat.Application.ProductService;
using Talabat.Core;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Infrastructure;
using Talabat.Infrastructure.Basket_Repository;
using Microsoft.AspNetCore.Mvc;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {


            #region Dependancey Injection

            Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

            Services.AddScoped(typeof(IProductService), typeof(ProductService));

            Services.AddScoped(typeof(IBaseketRepository), typeof(BasketRepository)); 

            Services.AddScoped(typeof(IOrderService), typeof(OrderService));

            Services.AddSingleton(typeof(IResponseCacheService), typeof(ResponseCacheService));

            Services.AddScoped(typeof(IPaymentService), typeof(PaymentService));

            #endregion


            Services.AddAutoMapper(typeof(MappingProfiles)); // Create Mapping Profiles in Helpers Folder to map Entities to Dtos and vice versa 

            // Configure API Behavior to return custom error response in case of validation errors
            Services.Configure<ApiBehaviorOptions>(option => {
                option.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToArray();
                    var errorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            });
            
            return Services;
        }
    }
}
