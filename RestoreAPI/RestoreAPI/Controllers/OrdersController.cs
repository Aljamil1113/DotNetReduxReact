using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestoreAPI.Data;
using RestoreAPI.DTOs;
using RestoreAPI.Entities.OrderAggregate;
using RestoreAPI.Extensions;

namespace RestoreAPI.Controllers
{
    public class OrdersController : BaseApiController
    {
        private readonly StoreContext context;

        public OrdersController(StoreContext _context)
        {
            context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetOrders()
        {
            return await context.Orders
                .ProjectOrderToOrderDto()
                .Where(x => x.BuyerId == User.Identity.Name)
                .ToListAsync();
        }

        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id) 
        {
            return await context.Orders
                .ProjectOrderToOrderDto()
                .Where(x => x.BuyerId == User.Identity.Name && x.Id == id)
                .FirstOrDefaultAsync();
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateOrder(CreateOrderDto orderDto)
        {
            var basket = await context.Baskets
                .RetrieveBasketWitgItems(User.Identity.Name)
                .FirstOrDefaultAsync();

            if(basket == null)
            {
                return BadRequest(new ProblemDetails { Title = "could not locate basket." });
            }

            var items = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var productItem = await context.Products.FindAsync(item.ProductId);
                var itemOrdered = new ProductItemOrdered
                {
                    ProductId = productItem.Id,
                    Name = productItem.Name,
                    PictureUrl = productItem.PictureUrl
                };

                var orderedItem = new OrderItem
                {
                    ItemOrdered = itemOrdered,
                    Price = productItem.Price,
                    Quantity = item.Quantity
                };
                items.Add(orderedItem);
                productItem.QuantityStock -= item.Quantity;
            }

            var subtotal = items.Sum(item => item.Price * item.Quantity);
            var deliveryFee = subtotal > 10000 ? 0 : 500;

            var order = new Order
            {
                OrderItems = items,
                BuyerId = User.Identity.Name,
                ShippingAddress = orderDto.ShippingAddress,
                SubTotal = subtotal,
                DeliveryFee = deliveryFee,
                PaymentIntentId = basket.PaymentIntentId
            };

            context.Orders.Add(order);
            context.Baskets.Remove(basket);

            if(orderDto.SaveAddress)
            {
                var user = await context.Users
                    .Include(a => a.Address)
                    .FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);

                var Address = new UserAddress
                {
                    FullName = order.ShippingAddress.FullName,
                    Address1 = order.ShippingAddress.Address1,
                    Address2 = order.ShippingAddress.Address2,
                    City = order.ShippingAddress.City,
                    State = order.ShippingAddress.State,
                    Zip = order.ShippingAddress.Zip,
                    Country = order.ShippingAddress.Country,
                };
                user.Address = Address;
                context.Update(user);
            }

            var result = await context.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetOrder", new { id = order.Id }, order.Id);

            return BadRequest("Problem creating order");
        }
    }
}
