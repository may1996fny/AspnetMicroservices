using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Queries.GetOrderList
{
	public class GetOrderListQueryHandler : IRequestHandler<GetOrderListQuery, List<OrdersVm>>
	{
		private readonly IOrderRepository orderRepository;
		private readonly IMapper mapper;

		public GetOrderListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
		{
			this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
			this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task<List<OrdersVm>> Handle(GetOrderListQuery request, 
			CancellationToken cancellationToken)
		{
			var orderList = await orderRepository.GetOrdersByUserName(request.UerName);
			var ordersVm = mapper.Map<List<OrdersVm>>(orderList);
			return ordersVm;
		}

	}
}