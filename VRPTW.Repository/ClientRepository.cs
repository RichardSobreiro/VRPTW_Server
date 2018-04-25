using Dapper;
using System.Collections.Generic;
using VRPTW.Domain.Entity;
using VRPTW.Domain.Interface.Repository;

namespace VRPTW.Repository
{
	public class ClientRepository : RepositoryBase, IClientRepository
	{
		public int CreateClient(Client client)
		{
			using (var connection = OpenConnection())
			{
				return connection.QuerySingleOrDefault<int>(CREATE_CLIENT, client);
			}
		}

		public void EditClient(Client client)
		{
			using (var connection = OpenConnection())
			{
				connection.Execute(EDIT_CLIENT, client);
			}
		}

		public List<Client> GetClientsByName(string clientName)
		{
			clientName = "%" + clientName + "%";
			using (var connection = OpenConnection())
			{
				return connection.Query<Client>(GET_CLIENTS_BY_NAME, new { ClientName = clientName }).AsList();
			}
		}

		public Client GetClientById(int clientId)
		{
			using (var connection = OpenConnection())
			{
				return connection.QuerySingleOrDefault<Client>(GET_CLIENT_BY_ID, new { ClientId = clientId });
			}
		}

		private const string CREATE_CLIENT = @"
			INSERT INTO Client (DateCreation, Name, DocumentNumber, DocumentType)
			VALUES (GETDATE(), @Name, @DocumentNumber, @DocumentType)
			SELECT SCOPE_IDENTITY()";

		private const string EDIT_CLIENT = @"
			UPDATE Client 
			SET 
				DateCreation = @DateCreation, 
				Name = @Name,  
				DocumentNumber = @DocumentNumber, 
				DocumentType = @DocumentType
			WHERE 
				ClientId = @ClientId";

		private const string GET_CLIENTS_BY_NAME = @"
			SELECT ClientId, DateCreation, Name, DocumentNumber, DocumentType
			FROM Client
			WHERE Name LIKE @ClientName";

		private const string GET_CLIENT_BY_ID = @"
			SELECT ClientId, DateCreation, Name, DocumentNumber, DocumentType
			FROM Client
			WHERE ClientId = @ClientId";
	}
}
