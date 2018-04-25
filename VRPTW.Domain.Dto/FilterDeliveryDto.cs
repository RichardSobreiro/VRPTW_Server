using System;
using VRPTW.CrossCutting.Enumerations;

namespace VRPTW.Domain.Dto
{
	public class FilterDeliveryDto
	{
		public DateTime dateDelivery { get; set; }
		public string clientName { get; set; }
		public int productType { get; set; }
		public double quantityProduct { get; set; }
		public StatusDelivery statusDelivery { get; set; }
	}
}
