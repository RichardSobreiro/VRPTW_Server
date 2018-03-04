using Dapper;
using System.Collections.Generic;
using VRPTW.Domain.Entity;
using VRPTW.Domain.Interface.Repository;

namespace VRPTW.Repository
{
	public class DeliveryRepository	: RepositoryBase, IDeliveryRepository
    {
		public int InsertDelivery(Delivery delivery)
		{
			using (var connection = OpenConnection())
			{
				return connection.QuerySingleOrDefault<int>(INSERT_DELIVERY, delivery); 
			}
		}

		public List<Delivery> GetDeliveries()
		{
			using (var connection = OpenConnection())
			{
				return connection.Query<Delivery>(GET_DELIVERIES).AsList();
			}
		}

		private const string INSERT_DELIVERY = @"
			INSERT INTO Delivery (DateDelivery, ClientId, ProductId, QuantityProduct)
			VALUES (@DateDelivery, @ClientId, @ProductId, @QuantityProduct)
			SELECT SCOPE_IDENTITY()";

		private const string GET_DELIVERIES = @"
			SELECT DeliveryId, DateDelivery, ClientId, ProductId, QuantityProduct 
			FROM Delivery";
    }
}
