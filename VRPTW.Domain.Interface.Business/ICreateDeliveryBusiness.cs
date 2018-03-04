using VRPTW.Domain.Dto;

namespace VRPTW.Domain.Interface.Business
{
	public interface ICreateDeliveryBusiness
	{
		int CreateFractionedDelivery(DeliveryDto deliveryDto);
	}
}
