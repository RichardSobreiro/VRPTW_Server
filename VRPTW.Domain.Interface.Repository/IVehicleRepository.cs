using System.Collections.Generic;
using VRPTW.Domain.Entity;

namespace VRPTW.Domain.Interface.Repository
{
	public interface IVehicleRepository
	{
		List<Vehicle> GetAvailableVehiclesByDepot(int depotId);
		int InsertVehicle(Vehicle vehicle);
	}
}
