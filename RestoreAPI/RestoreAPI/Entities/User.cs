using Microsoft.AspNetCore.Identity;
using RestoreAPI.Entities.OrderAggregate;

namespace RestoreAPI.Entities
{
    public class User : IdentityUser<int>
    {
        public UserAddress Address { get; set; }
    }
}
