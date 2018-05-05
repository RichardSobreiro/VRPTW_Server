using System;						   

namespace VRPTW.Domain.Dto
{
	public class FilterDeliveryDto
	{
		public DateTime? desiredDateInitial { get; set; }
		public DateTime? desiredDateFinal { get; set; }
		public string clientName { get; set; }
		public int? productType { get; set; }
		public double? quantityProductInitial { get; set; }
		public double? quantityProductFinal { get; set; }
		public char? valueStatus { get; set; }
	}
}
