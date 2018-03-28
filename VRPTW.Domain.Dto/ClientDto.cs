using System;

namespace VRPTW.Domain.Dto
{
	public class ClientDto
	{
		public int clientId { get; set; }
		public DateTime dateCreation { get; set; }
		public string name { get; set; }	 
		public int documentNumber { get; set; }
		public int documentType { get; set; }
		public AddressDto address { get; set; }
	}
}
