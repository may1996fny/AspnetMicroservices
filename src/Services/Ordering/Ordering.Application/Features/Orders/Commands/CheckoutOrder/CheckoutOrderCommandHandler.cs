using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
	public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand,int>
	{
		private readonly IOrderRepository repository;
		private readonly IMapper mapper;
		private readonly IEmailService emailService;
		private readonly ILogger<CheckoutOrderCommandHandler> logger;

		public CheckoutOrderCommandHandler(IOrderRepository repository, IMapper mapper, IEmailService emailService, ILogger<CheckoutOrderCommandHandler> logger)
		{
			this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
			this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			this.emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
		{
			var orderEntity = mapper.Map<Order>(request);
			var newOrder = await repository.AddAsync(orderEntity);
			await SendMail(newOrder);

			return newOrder.Id;
		}

		private async Task SendMail(Order order)
		{
			var email = new Email(){To="fbihda@gmail.com",Body=$"Order was created.", Subject="Order was created."};
			try
			{
				await emailService.SendEmail(email);
			}
			catch (Exception ex)
			{
				logger.LogError($"Order: {order.Id} faild due to an error with the email service: {ex.Message}");
			}			
		}
	}
}