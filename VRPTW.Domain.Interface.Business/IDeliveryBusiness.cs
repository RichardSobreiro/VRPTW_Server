using VRPTW.Domain.Dto;

namespace VRPTW.Domain.Interface.Business
{
	public interface IDeliveryBusiness
    {
		DeliveryDto ScheduleFractionedTrip(DeliveryDto deliveryTobeScheduled);
	}
}
