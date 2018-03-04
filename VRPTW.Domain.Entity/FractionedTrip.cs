using System;
using System.Collections.Generic;

namespace VRPTW.Domain.Entity
{
	public class FractionedTrip
	{
		public int FractionedTripId { get; set; }
		public DateTime TripTime { get; set; }
		public List<Client> Clients { get; set; }
		public DeliveryTruckTrip DeliveryTrip { get; set; }
	}
}
