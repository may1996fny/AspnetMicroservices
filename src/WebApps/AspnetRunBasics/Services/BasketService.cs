using AspnetRunBasics.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using AspnetRunBasics.Extensions;

namespace AspnetRunBasics.Services
{
	public class BasketService : IBasketService
	{
		private readonly HttpClient client;

		public BasketService(HttpClient client)
		{
			this.client = client ?? throw new ArgumentNullException(nameof(client));
		}

		public async Task<BasketModel> GetBasket(string userName)
		{
			var response = await client.GetAsync($"/Basket/{userName}");
			return await response.ReadContentAs<BasketModel>();
		}

		public async Task<BasketModel> UpdateBasket(BasketModel model)
		{
			var response = await client.PostAsJson($"/Basket", model);
			if (response.IsSuccessStatusCode)
				return await response.ReadContentAs<BasketModel>();
			else
			{
				throw new Exception("Something went wrong when calling api.");
			}
		}

		public async Task CheckoutBasket(BasketCheckoutModel model)
		{
			var response = await client.PostAsJson($"/Basket/Checkout", model);
			if (!response.IsSuccessStatusCode)
			{
				throw new Exception("Something went wrong when calling api.");
			}
		}
	}
}
