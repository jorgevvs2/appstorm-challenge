using CrudAppStorm.app.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CrudAppStorm.app.Database
{
    public class ProductDbSeed
    {
        public async static void ProductSeed(ProductContext context)
        {
            try
            {
                context.Database.Migrate();

            }
            catch
            {

            }
            if (!context.Product.Any())
            {

                var Products = new List<Product>
                {
                    new Product
                    {
                        Name = "Ball Joint",
                        Stock = 50,
                        Price = 123.11m,
                    },
                    new Product
                    {
                        Name = "Steering Box",
                        Stock = 23,
                        Price = 1000.52m,
                    },
                    new Product
                    {
                        Name = "Air Cond Compressor",
                        Stock = 89,
                        Price = 865.6m,
                    },
                    new Product
                    {
                        Name = "Windscreen wiper",
                        Stock = 786,
                        Price = 32.5m,
                    },
                    new Product
                    {
                        Name = "Drag Link",
                        Stock = 5,
                        Price = 423.8m,
                    },
                };

                context.Product.AddRange(Products);
                context.SaveChanges();
            }
        }
    }
}
