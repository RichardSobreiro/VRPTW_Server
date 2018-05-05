using Dapper;
using System.Collections.Generic;
using System.Linq;
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

			parameters.Add("DateDeliveryInitial", filterDelivery.DateDeliveryInitial);
			parameters.Add("DateDeliveryFinal", filterDelivery.DateDeliveryFinal);

			if (filterDelivery.ProductType.HasValue && filterDelivery.ProductType != 0)
			{
				query = query.Replace("{0}", " AND ProductType = @ProductType");
				parameters.Add("ProductType", filterDelivery.ProductType);
			}
			else
			{
				query = query.Replace("{0}", "");	
			}

			if(!string.IsNullOrEmpty(filterDelivery.ClientName))
			{
				filterDelivery.ClientName = "%" + filterDelivery + "%";
				query = query.Replace("{1}", " AND c.Name LIKE @ClientName");
				parameters.Add("ClientName", filterDelivery.ClientName);
			}
			else
			{
				query = query.Replace("{1}", "");
			}

			if(filterDelivery.QuantityProductInitial.HasValue && filterDelivery.QuantityProductInitial.Value > 0)
			{
				query = query.Replace("{2}", " AND QuantityProductInitial >= @QuantityProductInitial");
				parameters.Add("QuantityProductInitial", filterDelivery.QuantityProductInitial.Value);
			}
			else
			{
				query = query.Replace("{2}", "");
			}

			if (filterDelivery.QuantityProductFinal.HasValue && filterDelivery.QuantityProductFinal.Value > 0)
			{
				query = query.Replace("{3}", " AND QuantityProductFinal <= @QuantityProductFinal");
				parameters.Add("QuantityProductFinal", filterDelivery.QuantityProductFinal.Value);
			}
			else
			{
				query = query.Replace("{3}", "");
			}

			if (filterDelivery.ValueStatus.HasValue)
			{
				query = query.Replace("{4}", " AND StatusDelivery = @StatusDelivery");
				parameters.Add("StatusDelivery", filterDelivery.ValueStatus.Value);
			}
			else
			{
				query = query.Replace("{4}", "");
			}

			var lookup = new Dictionary<int, Delivery>();	  
			using (var connection = OpenConnection())
			{
				connection.Query<Delivery, Product,Client, StatusDelivery, Delivery>(query, (d, p, c, s) => {
					Delivery delivery;
					if (!lookup.TryGetValue(d.DeliveryId, out delivery))
					{
						lookup.Add(d.DeliveryId, delivery = d);
					}
					if(delivery.Product == null)
					{
						delivery.Product = new Product();
						delivery.Product.ProductType = p.ProductType;
						delivery.Product.DescriptionProduct = p.DescriptionProduct;
					}
					if (delivery.StatusDelivery == null)
					{
						delivery.StatusDelivery = new StatusDelivery();
						delivery.StatusDelivery.ValueStatus = s.ValueStatus;
						delivery.StatusDelivery.DescriptionStatus = s.DescriptionStatus;
					}
					if(delivery.Client == null)
					{
						delivery.Client = new Client();
						delivery.Client.ClientId = c.ClientId;
						delivery.Client.Name = c.Name; 
					}
					return delivery;
				}, 
				param: parameters,
				splitOn: "ProductType, ClientId, DescriptionStatus").AsQueryable();
			}

			return lookup.Values.AsList();
		}

		public List<StatusDelivery> GetStatusDeliveries()
		{
			using (var connection = OpenConnection())
			{
				return connection.Query<StatusDelivery>(GET_DELIVERIES_STATUS).AsList();
			}
		}

		public List<Delivery> GetDeliveriesById(List<int> deliveriesId)
		{
			var lookup = new Dictionary<int, Delivery>();
			using (var connection = OpenConnection())
			{
				connection.Query<Delivery, Product, Client, StatusDelivery, Delivery>(GET_DELIVERIES_BY_ID, 
					(d, p, c, s) => {
					Delivery delivery;
					if (!lookup.TryGetValue(d.DeliveryId, out delivery))
					{
						lookup.Add(d.DeliveryId, delivery = d);
					}
					if (delivery.Product == null)
					{
						delivery.Product = new Product();
						delivery.Product.ProductType = p.ProductType;
						delivery.Product.DescriptionProduct = p.DescriptionProduct;
					}
					if (delivery.StatusDelivery == null)
					{
						delivery.StatusDelivery = new StatusDelivery();
						delivery.StatusDelivery.ValueStatus = s.ValueStatus;
						delivery.StatusDelivery.DescriptionStatus = s.DescriptionStatus;
					}
					if (delivery.Client == null)
					{
						delivery.Client = new Client();
						delivery.Client.ClientId = c.ClientId;
						delivery.Client.Name = c.Name;
					}
					return delivery;
				}, param: deliveriesId,
				splitOn: "ProductType, ClientId, DescriptionStatus").AsQueryable();
			}
			return lookup.Values.AsList();
		}

		private const string INSERT_DELIVERY = @"
			INSERT INTO Delivery (DateDelivery, ClientId, ProductType, QuantityProduct)
			VALUES (@DateDelivery, @ClientId, @ProductType, @QuantityProduct)
			SELECT SCOPE_IDENTITY()";

		private const string GET_DELIVERIES_BY_FILTER = @"
			DECLARE @MIN DATETIME = FLOOR(CAST(@DateDeliveryInitial AS FLOAT))
			DECLARE @MAX DATETIME = FLOOR(CAST(@DateDeliveryFinal AS FLOAT))

			SELECT d.DeliveryId, d.DateDelivery, d.ProductType, d.QuantityProduct, 
				p.ProductType, p.DescriptionProduct, c.ClientId, c.Name, s.DescriptionStatus, s.ValueStatus
			FROM Delivery d
				INNER JOIN Client c ON c.ClientId = d.ClientId
				INNER JOIN Product p ON p.ProductType = d.ProductType
				INNER JOIN StatusDelivery s ON s.ValueStatus = d.StatusDelivery
			WHERE @MIN <= DateDelivery AND DateDelivery <= @MAX {0} {1} {2} {3} {4}";

		private const string GET_DELIVERIES_STATUS = @"
			SELECT ValueStatus, DescriptionStatus
			FROM StatusDelivery";

		private const string GET_DELIVERIES_BY_ID = @"
			SELECT d.DeliveryId, d.DateDelivery, d.ProductType, d.QuantityProduct, 
				p.ProductType, p.DescriptionProduct, c.ClientId, c.Name, s.DescriptionStatus, s.ValueStatus
			FROM Delivery d
				INNER JOIN Client c ON c.ClientId = d.ClientId
				INNER JOIN Product p ON p.ProductType = d.ProductType
				INNER JOIN StatusDelivery s ON s.ValueStatus = d.StatusDelivery
			WHERE d.DeliveryId IN @DeliveriesId";
    }
}
