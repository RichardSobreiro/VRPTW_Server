using System;
using System.Collections.Generic;

namespace VRPTW.Domain.Dto
{
	public class VehicleRouteDto
	{
		public int vehicleRouteId { get; set; }
		public DateTime dateCreation { get; set; }

		public DateTime dateScheduled { get; set; }
		public DateTime? departureTime { get; set; }
		public DateTime? estimatedTimeReturn { get; set; }

		public int vehicleId { get; set; }
		public DepotDto depot { get; set; }
		public List<SubRouteDto> subRoutes { get; set; }
	}
}
