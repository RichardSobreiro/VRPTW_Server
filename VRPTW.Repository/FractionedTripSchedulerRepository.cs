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
			SELECT DeliveryTruckTripId, DeliveryId, ProductType, QuantityProduct, TimeTrip, TimeArrivalClient, StatusTrip
			FROM DeliveryTruckTrip
			WHERE StatusTrip = 'S' AND ProductType = @ProductType
			ORDER BY DeliveryTruckTripId";
	}
}
