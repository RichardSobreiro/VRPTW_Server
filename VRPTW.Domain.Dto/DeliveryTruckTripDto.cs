using System;

namespace VRPTW.Domain.Dto
{
	public class DeliveryTruckTripDto
	{
		public int deliveryTruckTripId { get; set; }
		public int deliveryId { get; set; }
		public int productType { get; set; }
		public double quantityProduct { get; set; }
		public DateTime timeTrip { get; set; }
		public DateTime timeArrivalClient { get; set; }
	}
}
