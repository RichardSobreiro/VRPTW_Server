using Dapper;
using System.Collections.Generic;
using VRPTW.Domain.Entity;
using VRPTW.Domain.Interface.Repository;

namespace VRPTW.Repository
{
	public class ProductRepository : RepositoryBase, IProductRepository
	{
		public void CreateProduct(Product product)
		{
			using (var connection = OpenConnection())
			{
				connection.Execute(CREATE_PRODUCT, product);
			}
		}

		public void EditProduct(Product product)
		{
			using (var connection = OpenConnection())
			{
				connection.Execute(EDIT_PRODUCT, product);
			}
		}

		public List<Product> GetProducts()
		{
			using (var connection = OpenConnection())
			{
				return connection.Query<Product>(GET_PRODUCTS).AsList();
			}
		}

		private const string CREATE_PRODUCT = @"
			INSERT INTO Product (DescriptionProduct, UnityMeasurementId, Density, ProductProviderId)
			VALUES (@DescriptionProduct, @UnityMeasurementId, @Density, @ProductProviderId)";

		private const string EDIT_PRODUCT = @"
			UPDATE Product 
			SET
				DescriptionProduct = @DescriptionProduct,
				UnityMeasurementId = @UnityMeasurementId,
				Density = @Density,
				ProductProviderId = @ProductProviderId
			WHERE ProductId = @ProductId";

		private const string GET_PRODUCTS = @"
			SELECT ProductType, DescriptionProduct, UnityMeasurementId, Density, ProductProviderId
			FROM Product";
	}
}
