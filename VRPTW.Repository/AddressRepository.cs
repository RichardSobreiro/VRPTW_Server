using Dapper;
using VRPTW.Domain.Entity;
using VRPTW.Domain.Interface.Repository;

namespace VRPTW.Repository
{
	public class AddressRepository : RepositoryBase, IAddressRepository
	{
		public void InsertAddres(Address address)
		{
			using (var connection = OpenConnection())
			{
				connection.Execute(INSERT_ADDRESS, address);
			}
		}

		private static string INSERT_ADDRESS = @"
			INSERT INTO ADDRESS (Street, Number, Neighborhood, City, State, ProductProviderId, ClientId, DepotId)
			VALUES (@Street, @Number, @Neighborhood, @City, @State, @ProductProviderId, @ClientId, @DepotId)";
	}
}
