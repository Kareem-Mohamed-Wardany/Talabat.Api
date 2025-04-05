using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Repositories.Contract;
using Microsoft.AspNetCore.Mvc;

namespace Talabat.APIs.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBaseketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBaseketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var mappedBakset = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
            var createdOrUpdate = await _basketRepository.UpdateBasketAsync(mappedBakset);
            if (createdOrUpdate is null) return BadRequest(new ApiResponse(400));
            return Ok(createdOrUpdate);
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            return await _basketRepository.DeleteBasketAsync(id);
        }
    }
}
