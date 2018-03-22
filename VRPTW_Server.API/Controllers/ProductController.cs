using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using VRPTW.Domain.Dto;
using VRPTW.Domain.Interface.Business;

namespace VRPTW_Server.API.Controllers
{
	public class ProductController : BaseController
    {
		[HttpGet]
		[Route("product/products")]
		[ResponseType(typeof(List<ProductDto>))]
		public IHttpActionResult GetProducts()
		{
			try
			{
				return  Ok(_productBusiness.GetProducts());
			}
			catch(Exception e)
			{
				return (InternalServerError(e));
			}
		}

		[HttpPost]
		[Route("product/product")]
		[ResponseType(typeof(void))]
		public IHttpActionResult CreateProduct([FromBody] ProductDto productDto)
		{
			try
			{
				_productBusiness.CreateProduct(productDto);
				return Ok();
			}
			catch(Exception e)
			{
				return (InternalServerError(e));
			}
		}

		[HttpPut]
		[Route("product/product")]
		[ResponseType(typeof(void))]
		public IHttpActionResult EditProduct([FromBody] ProductDto productDto)
		{
			try
			{
				_productBusiness.EditProduct(productDto);
				return Ok();
			}
			catch (Exception e)
			{
				return (InternalServerError(e));
			}
		}

		public ProductController(IProductBusiness productBusiness)
		{
			_productBusiness = productBusiness;
		}

		private readonly IProductBusiness _productBusiness;
    }
}
