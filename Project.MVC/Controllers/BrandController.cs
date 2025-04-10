using AutoMapper;
using Project.API.Helpers;
using Project.BLL.Dtos.Product;
using System.Net.Http.Headers;


namespace Project.MVC.Controllers
{
    public class BrandController : Controller
    {
        private readonly IGenericRepository<Brand> _brandRepo;
        private readonly IMapper mapper;
        private readonly HttpClient _client;


        public BrandController(IGenericRepository<Brand> brandRepo, IMapper mapper)
        {
            _brandRepo = brandRepo;
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
            var brands = await _brandRepo.GetAllAsync();
            var Data = mapper.Map<IReadOnlyList<Brand>, IReadOnlyList<ProductBrandOrCategoryToReturnDto>>(brands);
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryOrBrandCreateDto model)
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToAction("Login", "Account");
            }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var check = _brandRepo.GetAllAsync().Result.Any(p => p.Name == model.Name);
            if (check)
            {
                ModelState.AddModelError("Name", "Brand Is Exist !");
                return View(model);
            }

            var brand = new Brand()
            {
                Name = model.Name,
                PictureUrl = DocumentSettings.UploadFile(model.Url)
            };
            await _brandRepo.AddAsync(brand);
            TempData["Create"] = "Brand is Created success";

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
            var brand = await _brandRepo.GetByIdAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            var brandDto = mapper.Map<ProductBrandOrCategoryEditDto>(brand);
            return View(brandDto);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductBrandOrCategoryEditDto BrandDto)
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToAction("Login", "Account");
            }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


            if (id != BrandDto.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var oldBrand = await _brandRepo.GetByIdAsync(id);


                try
                {
                    var check = _brandRepo.GetAllAsync().Result.Any(p => p.Name == BrandDto.Name && p.Id != BrandDto.Id);
                    if (check)
                    {
                        ModelState.AddModelError("Name", "Brand Is Exist");
                        var brand = mapper.Map<ProductBrandOrCategoryEditDto>(oldBrand);
                        return View(brand);
                    }

                    if (BrandDto.Url != null)
                    {
                        oldBrand.PictureUrl = DocumentSettings.UploadFile(BrandDto.Url);
                    }

                    oldBrand.Name = BrandDto.Name;

                    await _brandRepo.UpdateAsync(oldBrand);
                    TempData["Update"] = "Brand is Updated success";

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    var brandDto = mapper.Map<ProductBrandOrCategoryEditDto>(oldBrand);
                    return View(brandDto);
                }
            }



            return View(BrandDto);
        }




    }
}
