using Dapper;
using System.Collections.Generic;
using System.Linq;
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

		public List<VehicleRoute> GetVehicleRoutes(VehicleRouteFilter filter)
		{
			var parameters = new DynamicParameters();
			var query = GET_VEHICLE_ROUTES;

			parameters.Add("DesiredDateInitial", filter.DesiredDateInitial);
			parameters.Add("DesiredDateFinal", filter.DesiredDateFinal);

			if (filter.ProductType.HasValue && filter.ProductType != 0)
			{
				query = query.Replace("{0}", " AND vr.ProductType = @ProductType");
				parameters.Add("ProductType", filter.ProductType); 
			}
			else
			{
				query = query.Replace("{0}", "");
			}

			var lookup = new Dictionary<int, VehicleRoute>();
			using (var connection = OpenConnection())
			{
				connection.Query<VehicleRoute, Depot, SubRoute, VehicleRoute>(query, 
					(vr, d, sr) => 
					{
						VehicleRoute vehicleRoute;
						if(!lookup.TryGetValue(vr.VehicleRouteId, out vehicleRoute))
						{
							vehicleRoute.SubRoutes = new List<SubRoute>();
							lookup.Add(vr.VehicleRouteId, vehicleRoute = vr);
						}						
						if(vehicleRoute.Depot == null)
						{
							vehicleRoute.Depot = new Depot();
							vehicleRoute.Depot.DepotId = d.DepotId;
							vehicleRoute.Depot.DepotDescription = d.DepotDescription;
						}
						if (!vehicleRoute.SubRoutes.Any(subRoute => subRoute.SubRouteId == sr.SubRouteId))
						{
							vehicleRoute.SubRoutes.Add(sr);
						}
						return vehicleRoute;
					},
					param: parameters,
					splitOn: "DepotId, SubRouteId").AsQueryable();
			}
			return lookup.Values.AsList();
		}

		private const string INSERT_VEHICLE_ROUTE = @"
			INSERT INTO VehicleRoute (
				DateCreation,
				DateScheduled,
				DepartureTime,
				EstimatedTimeReturn,
				VehicleId,
				DepotId)
			VALUES (
				@DateCreation,
				@DateScheduled,
				@DepartureTime,
				@EstimatedTimeReturn,
				@VehicleId,
				@DepotId)
			SELECT SCOPE_IDENTITY()";

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

		private const string GET_VEHICLE_ROUTES = @"
			DECLARE @MIN DATETIME = FLOOR(CAST(@DesiredDateInitial AS FLOAT))
			DECLARE @MAX DATETIME = FLOOR(CAST(@DesiredDateFinal AS FLOAT))

			SELECT 
				vr.VehicleRouteId,
				vr.DateCreation,
				vr.DateScheduled,
				vr.DepartureTime,
				vr.EstimatedTimeReturn,
				vr.VehicleId,
				vr.DepotId,
				vr.ProductType,
				d.DepotId,
				d.DepotDescription,
				sr.SubRouteId,
				sr.VehicleRouteId,
				sr.AddressOriginId,
				sr.AddressDestinyId,
				sr.Distance,
				sr.Duration
			FROM 
				VehicleRoute vr
				INNER JOIN Depot d ON d.DepotId = vr.DepotId
				INNER JOIN SubRoute sr ON sr.VehicleRouteId = vr.VehicleRouteId
			WHERE 
				@MIN <= vr.DateCreation AND @MAX >= vr.DateCreation {0}";
	}
}
