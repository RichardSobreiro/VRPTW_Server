using System;
using System.Collections.Generic;

namespace VRPTW.Domain.Dto
{
	public class DeliveryDto
    {
		public int deliveryId { get; set; }
		public DateTime dateDelivery { get; set; }
		public int clientId { get; set; }
		public int productType { get; set; }
		public double quantityProduct { get; set; }
		public StatusDeliveryDto statusDelivery { get; set; }
		public ClientDto client { get; set; }
		public ProductDto product { get; set; }
		public List<DeliveryTruckTripDto> deliveriesTruckTips { get; set; }
		public AddressDto address { get; set; }
	}
}
