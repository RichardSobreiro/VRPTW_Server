using System;

namespace VRPTW.Domain.Entity
{
	public class Client
	{
		public int ClientId { get; set; }
		public DateTime DateCreation { get; set; }
		public string Name { get; set; }	   
		public int DocumentNumber { get; set; }
		public int DocumentType { get; set; }
		public Address Address { get; set; }
	}
}
