using System;
using System.Collections.Generic;		

namespace VRPTW.Domain.Entity
{
	public class Delivery
    {
		public int DeliveryId { get; set; }
		public DateTime DateDelivery { get; set; }
		public int ClientId { get; set; }
		public int ProductType { get; set; }
		public double QuantityProduct { get; set; }
		public StatusDelivery StatusDelivery { get; set; }
		public Address Address { get; set; }
		public Product Product { get; set; }
		public List<DeliveryTruckTrip> DeliveriesTruckTips { get; set; }
		public Client Client { get; set; }
	}
}
