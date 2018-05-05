using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using VRPTW.Domain.Dto;
using VRPTW.Domain.Interface.Business;

namespace VRPTW_Server.API.Controllers
{
	public class DeliveryController : BaseController
	{
		[HttpGet]
		[Route("delivery/deliveries")]
		[ResponseType(typeof(List<DeliveryDto>))]
		public IHttpActionResult GetDeliveriesByFilter(DateTime? desiredDateInitial = null, DateTime? desiredDateFinal = null,
			string clientName = null, int? productType = null, char? valueStatus = null, 
			float? quantityProductInitial = null, float? quantityProductFinal = null)
		{
			try
			{
				var deliveries = _deliveryBusiness.GetDeliveriesByFilter(new FilterDeliveryDto()
				{
					desiredDateInitial = desiredDateInitial,
					desiredDateFinal = desiredDateFinal,
					clientName = clientName,
					productType = productType,
					valueStatus = valueStatus,
					quantityProductInitial = quantityProductInitial,
					quantityProductFinal = quantityProductFinal
				});
				return Ok(deliveries);
			}
			catch(Exception e)
			{
				return InternalServerError(e);
			}
		}

		[HttpPost]
		[Route("fractioneddelivery")]
		[ResponseType(typeof(int))]
		public IHttpActionResult CreateFractionedDelivery([FromBody] DeliveryDto fractionedDeliveryDto)
		{
			try
			{																																	
				return Ok(_createDeliveryBusiness.CreateFractionedDelivery(fractionedDeliveryDto));
			}
			catch(Exception e)
			{
				return(InternalServerError(e));
			}
		}

		[HttpGet]
		[Route("delivery/statusdeliveries")]
		[ResponseType(typeof(List<StatusDeliveryDto>))]
		public IHttpActionResult GetStatusDeliveries()
		{
			try
			{
				var statusDeliveries = _deliveryBusiness.GetStatusDeliveries();
				return Ok(statusDeliveries);
			}
			catch(Exception e)
			{
				return InternalServerError(e);
			}
		}

		public DeliveryController(ICreateDeliveryBusiness createDeliveryBusiness, IDeliveryBusiness deliveryBusiness)
		{
			_createDeliveryBusiness = createDeliveryBusiness;
			_deliveryBusiness = deliveryBusiness;
		}

		private readonly ICreateDeliveryBusiness _createDeliveryBusiness;
		private readonly IDeliveryBusiness _deliveryBusiness;
	}
}
