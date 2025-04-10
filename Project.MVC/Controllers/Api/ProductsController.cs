using Project.API.Helpers;

namespace Project.MVC.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IGenericRepository<Product> _repoProduct;
        private readonly IGenericRepository<Image> _repoImages;


        public ProductsController(IGenericRepository<Product> repoProduct, IGenericRepository<Image> repoImage)
        {
            _repoProduct = repoProduct;
            _repoImages = repoImage;
        }
        [HttpDelete]

        public async Task<IActionResult> RemoveUser(int productId)
        {

            var ImagesList = await _repoImages.GetListByIdAsync(productId);
            var prod = await _repoProduct.GetByIdAsync(productId);
            if (prod == null)
                return NotFound();

            DocumentSettings.DeleteFile(prod.PictureUrl);
            foreach (var item in ImagesList)
            {
                DocumentSettings.DeleteFile(item.PictureUrl);
            }
            var result = await _repoProduct.DeleteAsync(prod);
            if (result == 0)
                return BadRequest(ModelState);

            return Ok("Deleted is Succeeful");
        }


    }
}