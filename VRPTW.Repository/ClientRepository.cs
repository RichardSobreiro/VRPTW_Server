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

		public List<Client> GetClients()
		{
			using (var connection = OpenConnection())
			{
				return connection.Query<Client>(GET_CLIENTS).AsList();
			}
		}

		private const string CREATE_CLIENT = @"
			INSERT_INTO Client (DateCreation, Name, DocumentNumber, DocumentType)
			VALUES (DateCreation, Name, DocumentNumber, DocumentType)
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

		private const string GET_CLIENTS = @"
			SELECT ClientId, DateCreation, Name, DocumentNumber, DocumentType
			FROM Client";
	}
}
