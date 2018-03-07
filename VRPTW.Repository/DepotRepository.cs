using Dapper;
using System.Collections.Generic;
using VRPTW.Domain.Entity;
using VRPTW.Domain.Interface.Repository;

namespace VRPTW.Repository
{
	public class DepotRepository : RepositoryBase, IDepotRepository
	{
		public List<Depot> SelectDepots()
		{
			using (var connection = OpenConnection())
			{
				return connection.Query<Depot>(SELECT_DEPOTS).AsList();
			}
		}

		private static string SELECT_DEPOTS = @"
			SELECT DepotId, Capacity, DepotDescription
			FROM Depot";
	}
}
