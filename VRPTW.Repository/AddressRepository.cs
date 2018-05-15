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

		public Address GetAddressByClientId(int clientId)
		{
			using (var connection = OpenConnection())
			{
				return connection.QuerySingleOrDefault<Address>(GET_ADDRESS_BY_CLIENT_ID, new { ClientId = clientId });
			}
		}

		public Address GetAddressByDepotId(int depotId)
		{
			using (var connection = OpenConnection())
			{
				return connection.QuerySingleOrDefault<Address>(GET_ADDRESS_BY_DEPOT_ID, new { DepotId = depotId });
			}
		}

		public Address GetAddressByAddressId(int addressId)
		{
			using (var connection = OpenConnection())
			{
				return connection.QuerySingleOrDefault<Address>(GET_ADDRESS_BY_ADDRESS_ID, new { AddressId = addressId });
			}
		}

		private static string INSERT_ADDRESS = @"
			INSERT INTO ADDRESS (Street, StreetNumber, Neighborhood, City, CountryState, ProductProviderId, 
				ClientId, DepotId, Latitude, Longitude)
			VALUES (@Street, @Number, @Neighborhood, @City, @State, @ProductProviderId, 
				@ClientId, @DepotId, @Latitude, @Longitude)";

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
				DepotId = @DepotId,
				Latitude = @Latitude, 
				Longitude = @Longitude
			WHERE 
				AddressId = @AddressId";

		private const string GET_ADDRESS_BY_CLIENT_ID = @"
			SELECT 
				AddressId,
				Street, 
				StreetNumber AS Number, 
				Neighborhood, 
				City, 
				CountryState AS State, 
				ProductProviderId, 
				ClientId, 
				DepotId,
				Latitude, 
				Longitude
			FROM 
				Address
			WHERE 
				ClientId = @ClientId";

		private const string GET_ADDRESS_BY_DEPOT_ID = @"
			SELECT 
				AddressId,
				Street, 
				StreetNumber AS Number, 
				Neighborhood, 
				City, 
				CountryState AS State, 
				ProductProviderId, 
				ClientId, 
				DepotId,
				Latitude, 
				Longitude
			FROM 
				Address
			WHERE 
				DepotId = @DepotId";

		private const string GET_ADDRESS_BY_ADDRESS_ID = @"
			SELECT 
				AddressId,
				Street, 
				StreetNumber AS Number, 
				Neighborhood, 
				City, 
				CountryState AS State, 
				ProductProviderId, 
				ClientId, 
				DepotId,
				Latitude, 
				Longitude
			FROM 
				Address
			WHERE 
				AddressId = @AddressId";
	}
}
