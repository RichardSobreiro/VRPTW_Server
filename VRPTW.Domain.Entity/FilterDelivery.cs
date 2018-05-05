using System;

namespace VRPTW.Domain.Entity
{
	public class FilterDelivery
	{
		public DateTime? DateDeliveryInitial { get; set; }
		public DateTime? DateDeliveryFinal { get; set; }
		public int? ProductType { get; set; }
		public string ClientName { get; set; }
		public double? QuantityProductInitial { get; set; }
		public double? QuantityProductFinal { get; set; }
		public char? ValueStatus { get; set; }
	}
}
