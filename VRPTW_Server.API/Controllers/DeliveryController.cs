using System;
using System.Web.Http;
using System.Web.Http.Description;
using VRPTW.Domain.Dto;
using VRPTW.Domain.Interface.Business;

namespace VRPTW_Server.API.Controllers
{
	public class DeliveryController : BaseController
	{
		[HttpGet]
		[Route("scheduledfractioneddelivery")]
		[ResponseType(typeof(void))]
		public IHttpActionResult ScheduleFractionedTrip([FromBody] DeliveryDto deliveryTobeScheduled)
		{
			try
			{
				_deliveryBusiness.ScheduleFractionedTrip(deliveryTobeScheduled);
				return Ok();
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

		public DeliveryController(ICreateDeliveryBusiness createDeliveryBusiness, IDeliveryBusiness deliveryBusiness)
		{
			_createDeliveryBusiness = createDeliveryBusiness;
			_deliveryBusiness = deliveryBusiness;
		}

		private readonly ICreateDeliveryBusiness _createDeliveryBusiness;
		private readonly IDeliveryBusiness _deliveryBusiness;
	}
}
