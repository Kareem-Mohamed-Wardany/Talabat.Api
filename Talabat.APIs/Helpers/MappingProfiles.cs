using AutoMapper;
using Talabat.Api.Dtos;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles : Profile
    {

        public MappingProfiles()
        {
            // Create Mapping Profiles
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d=>d.Brand,o=>o.MapFrom(s=>s.Brand.Name))
                .ForMember(d => d.Category, o => o.MapFrom(s => s.Category.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>()); // Custom Resolver to get Picture Url from Product Entity 

            CreateMap<CustomerBasketDto, CustomerBasket>();

            CreateMap<BasketItemDto, BasketItem>();

            CreateMap<Address, AddressDto>().ReverseMap();

            CreateMap<OrderAddressDto, OrderAddress>();

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>()); ;

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d=> d.DeliveryMethod, o=>o.MapFrom(s=>s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));

        }
    }
}
