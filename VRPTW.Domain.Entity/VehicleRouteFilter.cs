using System;

namespace VRPTW.Domain.Entity
{
	public class VehicleRouteFilter
	{
		public DateTime? DesiredDateInitial { get; set; }
		public DateTime? DesiredDateFinal { get; set; }
		public int? ProductType { get; set; }
	}
}
