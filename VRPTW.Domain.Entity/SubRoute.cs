using System;

namespace VRPTW.Domain.Entity
{
	public class SubRoute
	{
		public int SubRouteId { get; set; }
		public int VehicleRouteId { get; set; }
		public int AddressOriginId { get; set; }
		public int AddressDestinyId { get; set; }
		public Address AddressOrigin { get; set; }
		public Address AddressDestiny { get; set; }
		public double DemandOrigin { get; set; }
		public double DemandDestiny { get; set; }
		public double Distance { get; set; }
		public DateTime Duration { get; set; }
		public int SequenceNumber { get; set; }
	}
}
