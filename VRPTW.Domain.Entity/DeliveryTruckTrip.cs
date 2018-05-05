using System;								 

namespace VRPTW.Domain.Entity
{
	public class DeliveryTruckTrip
	{
		public int DeliveryTruckTripId { get; set; }
		public int DeliveryId { get; set; }
		public int ClientId { get; set; }
		public int  ProductType { get; set; }
		public double QuantityProduct { get; set; }
		public DateTime TimeTrip { get; set; }
		public DateTime TimeArrivalClient { get; set; }
		public string StatusTrip { get; set; }
		public Address Address { get; set; }

		public int ColumnIndex { get; set; }
	}
}
