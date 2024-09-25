using CrudAppStorm.app.Database;
using Microsoft.IdentityModel.Tokens;

namespace CrudAppStorm.app.Extensions
{
    public static class DataBaseSeedingExtension
    {
        public static IApplicationBuilder AddDataBaseSeedingService(this IApplicationBuilder app) 
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ProductContext>();
                ProductDbSeed.ProductSeed(context);
            }

            return app;
        }
    }
}
