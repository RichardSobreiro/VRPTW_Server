namespace VRPTW.Domain.Entity
{
	public class Address
	{
		public int AddressId { get; set; }
		public string Street { get; set; }
		public int Number { get; set; }
		public string Neighborhood { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public int? ProductProviderId { get; set; }
		public int? ClientId { get; set; }
		public string FormattedAddress
		{
			get
			{
				return Number + " " + Street + ", " + Neighborhood + ", " + City + ", " + State;
			}
		}
		public double? Latitude { get; set; }
		public double? Longitude { get; set; }
	}
}
