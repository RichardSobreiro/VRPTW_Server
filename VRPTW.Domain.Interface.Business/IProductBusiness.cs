using System.Collections.Generic;
using VRPTW.Domain.Dto;

namespace VRPTW.Domain.Interface.Business
{
	public interface IProductBusiness
	{
		void CreateProduct(ProductDto productDto);
		void EditProduct(ProductDto productDto);
		List<ProductDto> GetProducts();
	}
}
