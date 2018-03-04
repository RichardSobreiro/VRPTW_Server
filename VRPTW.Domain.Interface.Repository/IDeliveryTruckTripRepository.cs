using System.Collections.Generic;
using VRPTW.Domain.Entity;

namespace VRPTW.Domain.Interface.Repository
{
	public interface IDeliveryTruckTripRepository
	{
		int InsertDeliveryTruckTrip(DeliveryTruckTrip deliveryTruckTrip);
		List<DeliveryTruckTrip> GetDeliveriesTruckTripsByDeliveryId(int deliveryId);
	}
}
