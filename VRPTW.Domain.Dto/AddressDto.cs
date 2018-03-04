namespace VRPTW.Domain.Dto
{
	public class AddressDto
	{
		public int addressId { get; set; }
		public string street { get; set; }
		public int number { get; set; }
		public string neighborhood { get; set; }
		public string city { get; set; }
		public string state { get; set; }
		public int? productProviderId { get; set; }
		public int? clientId { get; set; }
	}
}
