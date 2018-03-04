namespace VRPTW.Domain.Entity
{
	public class Depot
	{
		public int DepotId { get; set; }
		public string DepotDescription { get; set; }
		public bool Active { get; set; }
		public Address Adress { get; set; }
	}
}
