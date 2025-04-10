
using Product = Project.DAL.Entities.Product;

namespace Project.API.Controllers
{
    //[Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;


        public ProductsController(ApplicationDbContext context, IGenericRepository<Product> productsRepo, IConfiguration configuration, IMapper mapper)
        {
            _productsRepo = productsRepo;
            _configuration = configuration;
            _mapper = mapper;
            _context = context;
        }
        [AllowAnonymous]
        [HttpGet(Router.ProductRouting.Pagination)]

        public async Task<ActionResult<Pagination<ProductsToReturnDto>>> GetProducts([FromQuery] ProductSpecParams productSpecParams)
        {

            var spec = new ProductWithCategoryAndBrandSpecification(productSpecParams);

            var products = await _productsRepo.GetAllWithSpecAsync(spec);

            var Data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductsToReturnDto>>(products);

            var countSpec = new ProductWithFiltersForCountSpecification(productSpecParams);

            var Count = await _productsRepo.GetCountAsync(countSpec);

            return Ok(new Pagination<ProductsToReturnDto>(productSpecParams.PageIndex, productSpecParams.PageSize, Count, Data));
        }



        [HttpGet(Router.ProductRouting.GetById)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var spec = new ProductWithCategoryAndBrandSpecification(id);
            var product = await _productsRepo.GetByIdWithSpecAsync(spec);
            var productDto = _mapper.Map<Product, ProductDto>(product);
            if (productDto == null) return NotFound(new ApiResponse(404));

            return Ok(productDto);
        }
    }
}
