using _2MStore.DAL.Context;
using _2MStore.DAL.Generic_Repository;
using _2MStore.DAL.InterFace;
using _2MStore.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace _2MStore.DAL.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {

        private readonly DbSet<Product> _products;
        public ProductRepository(AppDbContext context) : base(context)
        {

            _products = context.Set<Product>();

        }



    }

}
