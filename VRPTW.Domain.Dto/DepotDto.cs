namespace VRPTW.Domain.Dto
{
	public class DepotDto
	{
		public int depotId { get; set; }
		public string depotDescription { get; set; }
		public bool active { get; set; }
		public AddressDto adress { get; set; }
	}
}
