namespace Project.API.Controllers
{
    public class BrandController : Controller
    {

        private readonly IGenericRepository<Brand> BrandRepo;
        private readonly IMapper mapper;

        public BrandController(IGenericRepository<Brand> BrandRepo, IMapper mapper)
        {
            this.mapper = mapper;
            this.BrandRepo = BrandRepo;
        }
        [HttpGet(Router.BrandRouting.List)]
        public async Task<ActionResult<IReadOnlyList<Brand>>> Index()
        {
            var brands = await BrandRepo.GetAllAsync();
            var Data = mapper.Map<IReadOnlyList<Brand>, IReadOnlyList<ProductBrandOrCategoryToReturnDto>>(brands);

            return Ok(Data);
        }


    }
}
