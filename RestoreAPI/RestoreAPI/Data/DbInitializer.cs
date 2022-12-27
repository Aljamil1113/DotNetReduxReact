using RestoreAPI.Entities;

namespace RestoreAPI.Data
{
    public class DbInitializer
    {
        public static void Initialize(StoreContext context)
        {
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
