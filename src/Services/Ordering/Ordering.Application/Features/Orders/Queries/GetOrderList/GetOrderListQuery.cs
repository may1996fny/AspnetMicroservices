using System;
using System.Collections.Generic;
using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrderList
{
	public class GetOrderListQuery : IRequest<List<OrdersVm>>
	{
		public string UerName { get; set; }

		public GetOrderListQuery(string uerName)
		{
			UerName = uerName ?? throw new ArgumentNullException(nameof(uerName));
		}
	}
}