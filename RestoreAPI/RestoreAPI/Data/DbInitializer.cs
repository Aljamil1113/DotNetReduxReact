using Microsoft.AspNetCore.Identity;
using RestoreAPI.Entities;

namespace RestoreAPI.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(StoreContext context, UserManager<User> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new User
                {
                    UserName = "bob",
                    Email = "bob@test.com"
                };

                await userManager.CreateAsync(user, "P@ssw0rd");
                await userManager.AddToRoleAsync(user, "Member");

                var admin = new User
                {
                    UserName = "admin",
                    Email = "admin@test.com"
                };

                await userManager.CreateAsync(admin, "P@ssw0rd");
                await userManager.AddToRolesAsync(admin, new[] { "Member", "Admin" });
            }

            if (context.Products.Any()) return;

            var products = new List<Product>
            {
                new Product
                {
                    Name = "Green Angular Board 3000",
                    Description = "Nunc viverra imperdiet enim. Fusce est. Vivamus a tellus.",
                    Price = 150,
                    PictureUrl = "images/products/sb-ang2.png",
                    Brand =  "Angular",
                    Type = "Boots",
                    QuantityStock = 100
                }
            };

            foreach (var product in context.Products)
            {
                context.Products.Add(product);
            }

            context.SaveChanges();
        }
    }
}
