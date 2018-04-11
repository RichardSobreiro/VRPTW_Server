using Dapper;
using System.Collections.Generic;
using VRPTW.Domain.Entity;
using VRPTW.Domain.Interface.Repository;

namespace VRPTW.Repository
{
	public class FractionedTripSchedulerRepository : RepositoryBase, IFractionedTripRepository
	{
		public List<DeliveryTruckTrip> GetFractionedScheduledDeliveriesByProductType(int productType)
		{
			using (var connection = OpenConnection())
			{
				return connection.Query<DeliveryTruckTrip>(GET_FRACTIONED_SCHEDULED_TRIPS_BY_PRODUCT_TYPE, new { ProductType = productType }).AsList();
			}
		}

		private const string GET_FRACTIONED_SCHEDULED_TRIPS_BY_PRODUCT_TYPE = @"
			SELECT dt.DeliveryTruckTripId, dt.DeliveryId, dt.ProductType, dt.QuantityProduct, dt.TimeTrip, dt.TimeArrivalClient, dt.StatusTrip, d.ClientId
			FROM DeliveryTruckTrip dt
				INNER JOIN Delivery d ON d.DeliveryId = dt.DeliveryId
			WHERE dt.StatusTrip = 'P' AND dt.ProductType = @ProductType
			ORDER BY dt.DeliveryTruckTripId";
	}
}
