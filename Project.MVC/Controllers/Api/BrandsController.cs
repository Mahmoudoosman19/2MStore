using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.API.Helpers;
using Project.BLL.Dtos.Product;
using Project.DAL.Entities;

namespace Project.MVC.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IGenericRepository<Brand> _brandRepo;
        private readonly IMapper mapper;

        public BrandsController(IGenericRepository<Brand> brandRepo, IMapper mapper)
        {
            _brandRepo = brandRepo;
            this.mapper = mapper;
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {

            try
            {
                var brand = await _brandRepo.GetByIdAsync(id);
                DocumentSettings.DeleteFile(brand.PictureUrl);
                await _brandRepo.DeleteAsync(brand);
                return Ok(brand);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }
    }
}

