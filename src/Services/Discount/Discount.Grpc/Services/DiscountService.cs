using System;
using System.Threading.Tasks;
using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Discount.Grpc.Services
{
	public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
	{
		private readonly IDiscountRepository repository;
		private readonly ILogger<DiscountService> logger;
		private readonly IMapper mapper;

		public DiscountService(IDiscountRepository repository, ILogger<DiscountService> logger, IMapper mapper)
		{
			this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
			this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
		{
			var coupon = await repository.GetDiscount(request.ProductName);
			if (coupon == null)
				throw new RpcException(new Status(StatusCode.NotFound,
					$"Discount with ProductName={request.ProductName} is not found"));

			logger.LogInformation($"Discount is retrieved for ProductName: {coupon.ProductName}, Amount: {coupon.Amount}");

			var couponModel = mapper.Map<CouponModel>(coupon);
			return couponModel;
		}

		public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
		{
			var coupon = mapper.Map<Coupon>(request.Coupon);
			await repository.CreateDiscount(coupon);
			logger.LogInformation($"Discount is successfully created. ProductName: {coupon.ProductName}");

			var couponModel = mapper.Map<CouponModel>(coupon);
			return couponModel;
		}

		public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
		{
			var coupon = mapper.Map<Coupon>(request.Coupon);
			await repository.UpdateDiscount(coupon);
			logger.LogInformation($"Discount is successfully updated. ProductName: {coupon.ProductName}");

			var couponModel = mapper.Map<CouponModel>(coupon);

			return couponModel;
		}

		public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
		{
			var deleted = await repository.DeleteDiscount(request.ProductName);

			return new DeleteDiscountResponse { Success = deleted };
		}
	}
}