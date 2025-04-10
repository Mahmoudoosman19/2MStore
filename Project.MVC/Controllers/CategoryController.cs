using AutoMapper;
using Project.API.Helpers;
using Project.BLL.Dtos.Product;
using System.Net.Http.Headers;

namespace Project.MVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IGenericRepository<Category> _categoryRepo;
        private readonly IMapper mapper;
        private readonly HttpClient _client;
        public CategoryController(IGenericRepository<Category> categoryRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            this.mapper = mapper;
            _client = new HttpClient();

        }
        public async Task<ActionResult<IReadOnlyList<ProductBrandOrCategoryToReturnDto>>> Index()
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToAction("Login", "Account");
            }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToAction("Login", "Account");
            }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var categories = await _categoryRepo.GetAllAsync();
            var Data = mapper.Map<IReadOnlyList<Category>, IReadOnlyList<ProductBrandOrCategoryToReturnDto>>(categories);
            return View(Data);
        }
        public async Task<IActionResult> Create()
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToAction("Login", "Account");
            }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToAction("Login", "Account");
            }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryOrBrandCreateDto productCategoryToReturn)
        {


            var accessToken = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToAction("Login", "Account");
            }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            if (!ModelState.IsValid)
            {
                return View(productCategoryToReturn);
            }


            var check = _categoryRepo.GetAllAsync().Result.Any(p => p.Name == productCategoryToReturn.Name);
            if (check)
            {
                ModelState.AddModelError("Name", "Category Is Exist !");
                return View(productCategoryToReturn);
            }
            var category = new Category()
            {
                Name = productCategoryToReturn.Name,
                PictureUrl = DocumentSettings.UploadFile(productCategoryToReturn.Url)
            };
            await _categoryRepo.AddAsync(category);
            TempData["Create"] = "Category is Created success";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToAction("Login", "Account");
            }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToAction("Login", "Account");
            }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            var categoryDto = mapper.Map<ProductBrandOrCategoryEditDto>(category);
            return View(categoryDto);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductBrandOrCategoryEditDto CategoryDto)
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToAction("Login", "Account");
            }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (id != CategoryDto.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var oldcategory = await _categoryRepo.GetByIdAsync(id);
                try
                {
                    var check = _categoryRepo.GetAllAsync().Result.Any(p => p.Name == CategoryDto.Name && p.Id != CategoryDto.Id);
                    if (check)
                    {
                        ModelState.AddModelError("Name", "Category Is Exist");
                        var brand = mapper.Map<ProductBrandOrCategoryEditDto>(oldcategory);
                        return View(brand);
                    }
                    if (CategoryDto.Url != null)
                    {
                        oldcategory.PictureUrl = DocumentSettings.UploadFile(CategoryDto.Url);
                    }

                    oldcategory.Name = CategoryDto.Name;

                    await _categoryRepo.UpdateAsync(oldcategory);
                    TempData["Update"] = "Category is Updated success";

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    var categoryDto = mapper.Map<ProductBrandOrCategoryEditDto>(oldcategory);
                    return View(categoryDto);
                }
            }
            return View(CategoryDto);
        }


    }
}
