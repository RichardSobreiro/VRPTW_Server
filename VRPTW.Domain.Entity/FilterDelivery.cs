using System;
using VRPTW.CrossCutting.Enumerations;

namespace VRPTW.Domain.Entity
{
	public class FilterDelivery
	{
		public DateTime DateDelivery { get; set; }
		public int ProductType { get; set; }
		public string ClientName { get; set; }
		public double? QuantityProduct { get; set; }
		public StatusDelivery? StatusDelivery { get; set; }
	}
}
