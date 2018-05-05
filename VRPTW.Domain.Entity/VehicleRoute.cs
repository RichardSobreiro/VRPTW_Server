using System;
using System.Collections.Generic;

namespace VRPTW.Domain.Entity
{
	public class VehicleRoute
	{	
		public int VehicleRouteId { get; set; }
		public DateTime DateCreation { get; set; }	

		public DateTime DateScheduled { get; set; }
		public DateTime DepartureTime { get; set; }
		public DateTime EstimatedTimeReturn { get; set; }

		public int VehicleId { get; set; }
		public int DepotId { get; set; }
		public List<SubRoute> SubRoutes { get; set; } = new List<SubRoute>();
	}
}
