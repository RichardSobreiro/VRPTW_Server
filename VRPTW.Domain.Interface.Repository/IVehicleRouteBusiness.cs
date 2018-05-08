using System.Collections.Generic;
using VRPTW.Domain.Dto;

namespace VRPTW.Domain.Interface.Repository
{
	public interface IVehicleRouteBusiness
	{
		void ScheduleDeliveries(List<DeliveryDto> deliveriesToBeScheduled);
		List<VehicleRouteDto> GetVehicleRoutes(VehicleRouteFilterDto filter);
	}
}
