using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.API.Helpers;

namespace Project.MVC.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IGenericRepository<Category> _categoryRepo;
        private readonly IMapper mapper;

        public CategoryController(IGenericRepository<Category> categoryRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            this.mapper = mapper;
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {

            try
            {
                var brand = await _categoryRepo.GetByIdAsync(id);
                DocumentSettings.DeleteFile(brand.PictureUrl);
                await _categoryRepo.DeleteAsync(brand);
                return Ok(brand);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }
    }
}
