using System.Collections.Generic;
using System.Linq;

namespace Basket.API.Entities
{
	public class ShoppingCart
	{
		public string UserName { get; set; }
		public List<ShoppingCartItem> Items { get; } = new();

		public ShoppingCart(string userName)
		{
			UserName = userName;
		}

		public ShoppingCart()
		{
		}

		public decimal TotalPrice => Items.Sum(i => i.Quantity * i.Price);
	}
}