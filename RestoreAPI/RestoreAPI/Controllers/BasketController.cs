using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestoreAPI.Data;
using RestoreAPI.DTOs;
using RestoreAPI.Entities;

namespace RestoreAPI.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly StoreContext context;

        public BasketController(StoreContext _context)
        {
            context = _context;
        }

        [HttpGet(Name = "GetBasket")]
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
            var basket = await RetrieveBasket(); 

            if (basket == null) return NotFound();
            return MapBasketToDto(basket);
        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> AddItemToBasket(int productId, int quantity)
        {
            var basket = await RetrieveBasket();
            if (basket == null) basket = CreateBasket();
            var product = await context.Products.FindAsync(productId);

            if(product == null) return BadRequest(new ProblemDetails {  Title = "Product not found."});

            basket.AddItem(product, quantity);
            var result = await context.SaveChangesAsync() > 0;

            if(result)
            {
                return CreatedAtRoute("GetBasket", MapBasketToDto(basket));
            }

            return BadRequest(new ProblemDetails { Title = "Problem saving item to basket" });
            
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveBasketItem(int productId, int quantity)
        {
            var basket = await RetrieveBasket();

            if(basket == null) return NotFound();

            basket.RemoveItem(productId, quantity);

            var result = await context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem removing item from the basket" });
        }


        private async Task<Basket> RetrieveBasket()
        {
            var bt = await context.Baskets.Include(i => i.Items).ThenInclude(p => p.Product)
                            .FirstOrDefaultAsync();
            //x => x.BuyerId == Request.Cookies["buyerId"]
            return bt;
        }

        private Basket? CreateBasket()
        {
            var buyerId = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions { IsEssential = true, Expires = DateTime.Now.AddDays(30) };
            Response.Cookies.Append("buyerId", buyerId, cookieOptions);
            var basket = new Basket { BuyerId = buyerId };
            context.Baskets.Add(basket);
            return basket;
        }

        private BasketDto MapBasketToDto(Basket basket)
        {
            return new BasketDto
            {
                Id = basket.Id,
                BuyerId = basket.BuyerId,
                Items = basket.Items.Select(item => new BasketItemDto
                {
                    ProductId = item.ProductId,
                    Name = item.Product.Name,
                    Price = item.Product.Price,
                    PictureUrl = item.Product.PictureUrl,
                    Type = item.Product.Type,
                    Brand = item.Product.Brand,
                    Quantity = item.Quantity
                }).ToList()
            };
        }
    }
}
