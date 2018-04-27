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
		public IHttpActionResult GetDeliveriesByFilter([FromBody] FilterDeliveryDto filterDeliveryDto)
		{
			try
			{
				var deliveries = _deliveryBusiness.GetDeliveriesByFilter(filterDeliveryDto);
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

		[HttpPost]
		[Route("scheduledfractioneddelivery")]
		[ResponseType(typeof(void))]
		public IHttpActionResult ScheduleFractionedTrip([FromBody] List<DeliveryDto> deliveryTobeScheduled)
		{
			try
			{
				_deliveryBusiness.ScheduleDeliveries(deliveryTobeScheduled);
				return Ok();
			}
			catch(Exception e)
			{
				return InternalServerError(e);
			}
		}

		[HttpGet]
		[Route("statusdeliveries")]
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
