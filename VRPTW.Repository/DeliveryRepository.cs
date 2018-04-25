using Dapper;
using System;
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

		public List<Delivery> GetDeliveriesByFilter(FilterDelivery filterDelivery)
		{
			string query = GET_DELIVERIES_BY_FILTER;
			DynamicParameters parameters = new DynamicParameters();

			if (filterDelivery.DateDelivery != DateTime.MinValue)
			{
				query = query.Replace("{0}", " DateDelivery >= @MIN AND DateDelivery < @MAX ");
				parameters.Add("DateDelivery", filterDelivery.DateDelivery);
			}
			else
			{
				query = query.Replace("{0}", "");
			}

			if(filterDelivery.ProductType != 0)
			{
				query = query.Replace("{1}", " AND ProductType = @ProductType");
				parameters.Add("ProductType", filterDelivery.ProductType);
			}
			else
			{
				query = query.Replace("{1}", "");	
			}

			if(!string.IsNullOrEmpty(filterDelivery.ClientName))
			{
				query = query.Replace("{2}", " AND ClientName LIKE @ClientName");
				parameters.Add("ClientName", filterDelivery.ClientName);
			}
			else
			{
				query = query.Replace("{2}", "");
			}

			if(filterDelivery.QuantityProduct.HasValue && filterDelivery.QuantityProduct.Value > 0)
			{
				query = query.Replace("{3}", " AND QuantityProduct >= @QuantityProduct");
				parameters.Add("QuantityProduct", filterDelivery.QuantityProduct.Value);
			}
			else
			{
				query = query.Replace("{3}", "");
			}

			if(filterDelivery.StatusDelivery.HasValue)
			{
				query = query.Replace("{4}", " AND StatusDelivery = @StatusDelivery");
				parameters.Add("StatusDelivery", filterDelivery.StatusDelivery.Value);
			}
			else
			{
				query = query.Replace("{4}", "");
			}

			using (var connection = OpenConnection())
			{
				return connection.Query<Delivery>(query, parameters).AsList();
			}
		}

		private const string INSERT_DELIVERY = @"
			INSERT INTO Delivery (DateDelivery, ClientId, ProductType, QuantityProduct)
			VALUES (@DateDelivery, @ClientId, @ProductType, @QuantityProduct)
			SELECT SCOPE_IDENTITY()";

		private const string GET_DELIVERIES_BY_FILTER = @"
			DECLARE @MIN DATETIME = FLOOR(CAST(@DateDelivery AS FLOAT))
			DECLARE @MAX DATETIME = DATEADD(DAY, 1, @MIN)

			SELECT DeliveryId, DateDelivery, ClientId, ProductId, QuantityProduct 
			FROM Delivery
			WHERE {0}";
    }
}
