using Dapper;
using System.Collections.Generic;
using VRPTW.Domain.Entity;
using VRPTW.Domain.Interface.Repository;

namespace VRPTW.Repository
{
	public class DeliveryTruckTripRepository : RepositoryBase, IDeliveryTruckTripRepository
	{
		public int InsertDeliveryTruckTrip(DeliveryTruckTrip deliveryTruckTrip)
		{
			using (var connection = OpenConnection())
			{
				return connection.QuerySingleOrDefault<int>(INSERT_DELIVERY_TRUCK_TRIP, deliveryTruckTrip);
			}
		}

		public List<DeliveryTruckTrip> GetDeliveriesTruckTripsByDeliveryId(int deliveryId)
		{
			using (var connection = OpenConnection())
			{
				return connection.Query<DeliveryTruckTrip>(SELECT_DELIVERY_TRUCK_TRIP_BY_ID, new { DeliveryId = deliveryId }).AsList();
			}
		}

		private const string INSERT_DELIVERY_TRUCK_TRIP = @"
			INSERT INTO DeliveryTruckTrip (DeliveryId, ProductType, QuantityProduct, StatusTrip) 
			Values (@DeliveryId, @ProductType, @QuantityProduct, 'P')";

		private const string SELECT_DELIVERY_TRUCK_TRIP_BY_ID = @"
			SELECT DeliveryTruckTripId, DeliveryId, ProductType, QuantityProduct, TimeTrip, TimeArrivalClient, StatusTrip
			FROM DeliveryTruckTrip
			WHERE DeliveryId = @DeliveryId";
	}
}
