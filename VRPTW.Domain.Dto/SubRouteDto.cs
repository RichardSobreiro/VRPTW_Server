using System;

namespace VRPTW.Domain.Dto
{
	public class SubRouteDto
	{
		public int subRouteId { get; set; }
		public int vehicleRouteId { get; set; }
		public AddressDto addressOrigin { get; set; }
		public AddressDto addressDestiny { get; set; }
		public double distance { get; set; }
		public DateTime duration { get; set; }
	}
}
