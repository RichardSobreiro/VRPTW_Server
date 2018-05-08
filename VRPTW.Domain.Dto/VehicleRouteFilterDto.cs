using System;

namespace VRPTW.Domain.Dto
{
	public class VehicleRouteFilterDto
	{
		public DateTime? desiredDateInitial { get; set; }
		public DateTime? desiredDateFinal { get; set; }
		public int? productType { get; set; }
	}
}
