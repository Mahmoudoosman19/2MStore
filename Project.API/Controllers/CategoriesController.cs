

namespace Project.API.Controllers
{
    public class CategoriesController : ControllerBase
    {
        private readonly IGenericRepository<Category> CategoryRepo;
        private readonly IMapper mapper;

        public CategoriesController(IGenericRepository<Category> CategoryRepo, IMapper mapper)
        {
            this.CategoryRepo = CategoryRepo;
            this.mapper = mapper;
        }

        [HttpGet(Router.CategoryRouting.List)]
        public async Task<ActionResult<IReadOnlyList<Category>>> Categories()
        {
            var Categories = await CategoryRepo.GetAllAsync();
            var Data = mapper.Map<IReadOnlyList<Category>, IReadOnlyList<ProductBrandOrCategoryToReturnDto>>(Categories);

            return Ok(Data);
        }

    }
}
