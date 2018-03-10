using Dapper;
using System.Collections.Generic;
using VRPTW.Domain.Entity;
using VRPTW.Domain.Interface.Repository;

namespace VRPTW.Repository
{
	public class VehicleRepository : RepositoryBase, IVehicleRepository
	{
		public List<Vehicle> GetAvailableVehiclesByDepot(int depotId)
		{
			using (var connection = OpenConnection())
			{
				return connection.Query<Vehicle>(GET_AVAILABLE_VEHICLES, new { DepotId = depotId }).AsList();
			}
		}

		public int InsertVehicle(Vehicle vehicle)
		{
			using (var connection = OpenConnection())
			{
				return connection.QuerySingleOrDefault<int>(INSERT_VEHICLE, vehicle);
			}
		}

		private static string GET_AVAILABLE_VEHICLES = @"SELECT VehicleId, Available, DepotId WHERE DepotId = @DepotId";

		private static string INSERT_VEHICLE = @"INSERT INTO Vehicle (Available, DepotId) VALUES (1, @DepotId)";
	}
}
