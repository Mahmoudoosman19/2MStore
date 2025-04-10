using Microsoft.Extensions.Logging;
using Project.DAL.Entities;
using Project.DAL.Entities.Order;
using System.Text.Json;

namespace Project.DAL.Data
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.Brands.Any())
                {
                    var brandsData = File.ReadAllText("../Project.DAL/Data/SeedData/brands.json");
                    var brands = JsonSerializer.Deserialize<List<Brand>>(brandsData);

                    foreach (var brand in brands)
                    {
                        context.Set<Brand>().Add(brand);
                    }
                    await context.SaveChangesAsync();

                }


                if (!context.Categories.Any())
                {
                    var CategoriesData = File.ReadAllText("../Project.DAL/Data/SeedData/types.json");
                    var Categories = JsonSerializer.Deserialize<List<Category>>(CategoriesData);

                    foreach (var Category in Categories)
                    {
                        context.Set<Category>().Add(Category);
                    }
                    await context.SaveChangesAsync();

                }
                if (!context.Products.Any())
                {
                    var productsData = File.ReadAllText("../Project.DAL/Data/SeedData/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                    foreach (var product in products)
                    {
                        context.Set<Product>().Add(product);
                    }
                    await context.SaveChangesAsync();

                }

                if (!context.DeliveryMethods.Any())
                {
                    var deliveryData = File.ReadAllText("../Project.DAL/Data/SeedData/delivery.json");
                    var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);

                    foreach (var deliveryMethod in DeliveryMethods)
                    {
                        context.Set<DeliveryMethod>().Add(deliveryMethod);
                    }
                    await context.SaveChangesAsync();

                }

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<ApplicationDbContextSeed>();
                logger.LogError(ex, ex.Message);
            }

        }
    }
}
