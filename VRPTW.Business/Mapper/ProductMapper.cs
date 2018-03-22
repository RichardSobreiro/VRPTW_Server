using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRPTW.Domain.Dto;
using VRPTW.Domain.Entity;

namespace VRPTW.Business.Mapper
{
	public static class ProductMapper
	{
		public static Product CreateEntity(this ProductDto productDto)
		{
			return new Product()
			{
				ProductId = productDto.productId,
				DescriptionProduct = productDto.descriptionProduct,
				UnityMeasurementId = productDto.unityMeasurementId,
				Density = productDto.density,
				ProductProviderId = productDto.productProviderId	
			};
		}

		public static ProductDto CreateDto(this Product product)
		{
			return new ProductDto()
			{
				productId = product.ProductId,
				descriptionProduct = product.DescriptionProduct,
				unityMeasurementId = product.UnityMeasurementId,
				density = product.Density,
				productProviderId = product.ProductProviderId
			};
		}

		public static List<ProductDto> CreateDto(this List<Product> products)
		{
			return products.ConvertAll(product => product.CreateDto());
		} 
	}
}
