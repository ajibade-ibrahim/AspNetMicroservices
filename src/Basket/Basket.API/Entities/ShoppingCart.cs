using System.Collections.Generic;
using System.Linq;

namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
        }

        public ShoppingCart(string userName)
        {
            UserName = userName;
        }

        public List<ShoppingCartItem> Items { get; set; } = new();

        public decimal TotalPrice
        {
            get { return Items.Sum(item => item.Price * item.Quantity); }
        }

        public string UserName { get; set; }
    }
}