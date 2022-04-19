using AspnetRunBasics.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AspnetRunBasics.Extensions;

namespace AspnetRunBasics.Services
{
	public class OrderService : IOrderService
	{
		private readonly HttpClient client;

		public OrderService(HttpClient client)
		{
			this.client = client ?? throw new ArgumentNullException(nameof(client));
		}

		public async Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName)
		{
			var response = await client.GetAsync($"/Order/{userName}");
			return await response.ReadContentAs<List<OrderResponseModel>>();
		}
	}
}
