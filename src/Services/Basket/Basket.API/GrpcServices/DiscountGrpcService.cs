using System;
using System.Threading.Tasks;
using Discount.Grpc.Protos;

namespace Basket.API.GrpcServices
{
	public class DiscountGrpcService
	{
		private readonly DiscountProtoService.DiscountProtoServiceClient discountProtoService;

		public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
		{
			this.discountProtoService =
				discountProtoService ?? throw new ArgumentNullException(nameof(discountProtoService));
		}

		public async Task<CouponModel> GetDiscount(string productName)
		{
			var discountRequest = new GetDiscountRequest { ProductName = productName };
			return await discountProtoService.GetDiscountAsync(discountRequest);
		}
	}
}