using System.Collections.Generic;
using VRPTW.Domain.Entity;

namespace VRPTW.Domain.Interface.Repository
{
	public interface IProductRepository
	{
		void CreateProduct(Product product);
		void EditProduct(Product product);
		List<Product> GetProducts();
	}
}
