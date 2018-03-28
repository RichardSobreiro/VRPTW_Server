using Dapper;
using VRPTW.Domain.Entity;
using VRPTW.Domain.Interface.Repository;

namespace VRPTW.Repository
{
	public class AddressRepository : RepositoryBase, IAddressRepository
	{
		public void CreateAddres(Address address)
		{
			using (var connection = OpenConnection())
			{
				connection.Execute(INSERT_ADDRESS, address);
			}
		}		   

		public void EditAddress(Address address)
		{
			using (var connection = OpenConnection())
			{
				connection.Execute(EDIT_CLIENT_ADDRESS, address);
			}
		}

		private static string INSERT_ADDRESS = @"
			INSERT INTO ADDRESS (Street, Number, Neighborhood, City, State, ProductProviderId, ClientId, DepotId)
			VALUES (@Street, @Number, @Neighborhood, @City, @State, @ProductProviderId, @ClientId, @DepotId)";

		private static string EDIT_CLIENT_ADDRESS = @"
			UPDATE Client
			SET 
				Street = @Street, 
				Number = @Number, 
				Neighborhood = @Neighborhood, 
				City = @City, 
				State = @State, 
				ProductProviderId = @ProductProviderId, 
				ClientId = @ClientId, 
				DepotId = @DepotId
			WHERE 
				AddressId = @AddressId";
	}
}
