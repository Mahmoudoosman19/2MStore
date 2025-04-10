using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Project.API.Helpers;
using Project.BLL.Dtos.Product;
using System.Net.Http.Headers;

namespace Project.MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IGenericRepository<Product> _repoProduct;
        private readonly IGenericRepository<Brand> _repobrand;
        private readonly IGenericRepository<Category> _repoCategory;
        private readonly IGenericRepository<Image> _repoImage;
        private readonly IMapper _mapper;

        public ProductController(IGenericRepository<Product> repoProduct, IMapper mapper,
            IGenericRepository<Brand> repobrand, IGenericRepository<Category> repoCategory, IGenericRepository<Image> repoImage)
        {
            _repoProduct = repoProduct;
            _mapper = mapper;
            _repobrand = repobrand;
            _repoCategory = repoCategory;
            _repoImage = repoImage;

        }
        HttpClient _client = new HttpClient();


        public async Task<IActionResult> Index([FromQuery] ProductSpecParams productSpecParams)
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToAction("Login", "Account");
            }

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Load Brands and Categories before making the product request
            var brandsResponse = await _client.GetAsync("https://localhost:7276/Api/v1/Brand/List");
            var categoriesResponse = await _client.GetAsync("https://localhost:7276/Api/v1/Categories/List");

            if (brandsResponse.IsSuccessStatusCode && categoriesResponse.IsSuccessStatusCode)
            {
                // Deserialize brands and categories
                var brandsJson = await brandsResponse.Content.ReadAsStringAsync();
                var categoriesJson = await categoriesResponse.Content.ReadAsStringAsync();

                var brands = JsonConvert.DeserializeObject<List<Brand>>(brandsJson);
                var categories = JsonConvert.DeserializeObject<List<Category>>(categoriesJson);

                // Store in ViewBag
                ViewBag.Brands = brands;
                ViewBag.Categories = categories;
            }
            else
            {
                // If there's an issue loading brands or categories, log the error and return the error view
                return View("Error");
            }

            // Build the API query string dynamically based on the provided parameters
            var query = $"https://localhost:7276/Api/v1/Products/Pagination?PageIndex={productSpecParams.PageIndex}&PageSize=5";

            if (productSpecParams.BrandId.HasValue)
            {
                query += $"&BrandId={productSpecParams.BrandId}";
            }

            if (productSpecParams.CategoryId.HasValue)
            {
                query += $"&CategoryId={productSpecParams.CategoryId}";
            }

            if (!string.IsNullOrEmpty(productSpecParams.Search))
            {
                query += $"&Search={productSpecParams.Search}";
            }

            var response = await _client.GetAsync(query);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserialize into PaginatedResponse<ProductDto>
                var paginatedProducts = JsonConvert.DeserializeObject<Pagination<ProductsToReturnDto>>(jsonResponse);

                // Calculate total pages dynamically
                int totalPages = (int)Math.Ceiling((double)paginatedProducts.Count / productSpecParams.PageSize);

                ViewBag.PageIndex = paginatedProducts.PageIndex;
                ViewBag.PageSize = paginatedProducts.PageSize;
                ViewBag.TotalPages = totalPages;

                return View(paginatedProducts.Data); // Pass the list of products to the view
            }

            return View("Error");
        }

        [HttpGet]
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
            await DataForBrandAndCategory();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDto model)
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
            if (!ModelState.IsValid)
            {
                await DataForBrandAndCategory();

                return View(model);
            }
            model.PictureUrl = DocumentSettings.UploadFile(model.Url);

            var product = new Product()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                PictureUrl = model.PictureUrl,
                Discount = model.Discount,
                Count = model.Count,
                PriceBeforeDiscount = model.PriceBeforeDiscount,
                BrandId = model.BrandId,
                CategoryId = model.CategoryId,


            };
            await _repoProduct.AddAsync(product);
            if (model.ImagesUrl.Count > 0)
            {
                foreach (var file in model.ImagesUrl)
                {
                    var fileName = DocumentSettings.UploadFile(file);

                    var image = new Image
                    {
                        PictureUrl = fileName,
                        ProductId = product.Id
                    };
                    await _repoImage.AddAsync(image);

                }
            }
            else
            {

                return View(model);
            }


            TempData["Create"] = "Product is Created success";

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
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
            if (id == null)
                return NotFound();
            var spec = new ProductWithCategoryAndBrandSpecification(id);

            var prod = await _repoProduct.GetByIdWithSpecAsync(spec);

            if (prod == null)
                return NotFound();

            var mapping = _mapper.Map<EditProductDto>(prod);

            await DataForBrandAndCategory();


            return View(mapping);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, EditProductDto model)
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToAction("Login", "Account");
            }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var ImagesList = await _repoImage.GetListByIdAsync(id);
            var img = ImagesList.Select(x => new ImageToReturnDto
            {
                Id = x.Id,
                Name = x.PictureUrl,

            });
            model.Images = img.ToList();

            if (!ModelState.IsValid)
            {

                await DataForBrandAndCategory();

                return View(model);
            }

            var spec = new ProductWithCategoryAndBrandSpecification(id);
            var OldProduct = await _repoProduct.GetByIdWithSpecAsync(spec);

            if (model.Url != null)
            {
                model.PictureUrl = DocumentSettings.UploadFile(model.Url);
                OldProduct.PictureUrl = model.PictureUrl;


            }



            OldProduct.Description = model.Description;
            OldProduct.Count = model.Count;
            OldProduct.BrandId = model.BrandId;
            OldProduct.CategoryId = model.CategoryId;
            OldProduct.Name = model.Name;
            OldProduct.PriceBeforeDiscount = model.PriceBeforeDiscount;
            OldProduct.Images = ImagesList.ToList();
            await _repoProduct.UpdateAsync(OldProduct);

            if (model.ImagesUrl?.Count > 0)
            {
                OldProduct.Images = null;
                foreach (var file in model.ImagesUrl)
                {
                    var fileName = DocumentSettings.UploadFile(file);

                    var image = new Image
                    {
                        PictureUrl = fileName,
                        ProductId = OldProduct.Id
                    };
                    await _repoImage.UpdateAsync(image);

                }
            }

            TempData["Update"] = "Product is Updated success";

            return RedirectToAction(nameof(Index));
        }

        private async Task DataForBrandAndCategory()
        {
            var brands = await _repobrand.GetAllAsync();
            var Categories = await _repoCategory.GetAllAsync();
            ViewBag.Brands = new SelectList(brands, "Id", "Name");
            ViewBag.Categories = new SelectList(Categories, "Id", "Name");
        }


    }
}