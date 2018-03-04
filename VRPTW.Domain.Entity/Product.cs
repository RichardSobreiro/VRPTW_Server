namespace VRPTW.Domain.Entity
{
	public class Product
	{
		public int ProductId { get; set; }
		public string DescriptionProduct { get; set; }
		public int UnityMeasurementId { get; set; }
		public float Density { get; set; }
		public int ProductProviderId { get; set; }
	}
}
