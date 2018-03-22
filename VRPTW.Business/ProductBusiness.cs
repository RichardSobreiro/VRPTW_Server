using System;
using System.Collections.Generic;
using VRPTW.Business.Mapper;
using VRPTW.Domain.Dto;
using VRPTW.Domain.Interface.Business;
using VRPTW.Domain.Interface.Repository;

namespace VRPTW.Business
{
	public class ProductBusiness : IProductBusiness
	{
		public void CreateProduct(ProductDto productDto)
		{
			var product = productDto.CreateEntity();
			_productRepository.CreateProduct(product);
		}

		public void EditProduct(ProductDto productDto)
		{
			var product = productDto.CreateEntity();
			_productRepository.EditProduct(product);
		}

		public List<ProductDto> GetProducts()
		{
			var products = _productRepository.GetProducts();
			var productsDto = products.CreateDto();
			return productsDto;
		}

		public ProductBusiness(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		private readonly IProductRepository _productRepository;
	}
}
