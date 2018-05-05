using Dapper;
using VRPTW.Domain.Entity;
using VRPTW.Domain.Interface.Repository;

namespace VRPTW.Repository
{
	public class VehicleRouteRepository : RepositoryBase, IVehicleRouteRepository
	{
		public int InsertVehicleRoute(VehicleRoute vehicleRoute)
		{
			using (var connection = OpenConnection())
			{
				return connection.QuerySingleOrDefault<int>(INSERT_VEHICLE_ROUTE, vehicleRoute);
			}
		}

		public int InsertSubRoute(SubRoute subRoute)
		{
			using (var connection = OpenConnection())
			{
				return connection.QuerySingleOrDefault<int>(INSERT_SUB_ROUTE, subRoute);
			}
		}

		private const string INSERT_VEHICLE_ROUTE = @"
			INSERT INTO VehicleRoute
				DateCreation
				DateScheduled
				DepartureTime
				EstimatedTimeReturn
				VehicleId
				DepotId
			VALUES
				DateCreation,
				DateScheduled,
				DepartureTime,
				EstimatedTimeReturn,
				VehicleId,
				DepotId
			SELECT SCOPE_IDENTY()";

		private const string INSERT_SUB_ROUTE = @"
			INSERT INTO SubRoute
				VehicleRouteId
				AddressOriginId
				AddressDestinyId
				Distance
				Duration
			VALUES
				VehicleRouteId,
				AddressOriginId,
				AddressDestinyId,
				Distance,
				Duration
			SELECT SCOPE_IDENTITY()";
	}
}
