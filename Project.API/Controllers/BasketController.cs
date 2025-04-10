using Project.BLL.Dtos.Basket;

namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;

        private readonly IMapper _Mapper;

        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _Mapper = mapper;
        }


        [HttpGet] // Get /api/basket?id=
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return basket ?? new CustomerBasket(id);    // if the basket is expired
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var mappedBasket = _Mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
            var createdOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(mappedBasket);
            if (createdOrUpdatedBasket is null) return BadRequest(new ApiResponse(400));

            return Ok(createdOrUpdatedBasket);

        }


        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);
        }
    }
}
