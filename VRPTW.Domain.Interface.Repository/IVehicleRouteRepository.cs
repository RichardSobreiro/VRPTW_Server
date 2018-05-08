using System.Collections.Generic;
using VRPTW.Domain.Entity;

namespace VRPTW.Domain.Interface.Repository
{
	public interface IVehicleRouteRepository
	{
		int InsertVehicleRoute(VehicleRoute vehicleRoute);
		int InsertSubRoute(SubRoute subRoute);
		List<VehicleRoute> GetVehicleRoutes(VehicleRouteFilter filter);
	}
}
