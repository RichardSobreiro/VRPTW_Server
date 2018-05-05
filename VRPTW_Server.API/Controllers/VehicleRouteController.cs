using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using VRPTW.Domain.Dto;
using VRPTW.Domain.Interface.Repository;

namespace VRPTW_Server.API.Controllers
{
	public class VehicleRouteController : ApiController
    {
		[HttpPost]
		[Route("scheduledfractioneddelivery")]
		[ResponseType(typeof(void))]
		public IHttpActionResult ScheduleFractionedTrip([FromBody] List<DeliveryDto> deliveriesSelected)
		{
			try
			{
				_vehicleRouteBusiness.ScheduleDeliveries(deliveriesSelected);
				return Ok();
			}
			catch (Exception e)
			{
				return InternalServerError(e);
			}
		}

		public VehicleRouteController(IVehicleRouteBusiness vehicleRouteBusiness)
		{
			_vehicleRouteBusiness = vehicleRouteBusiness;
		}

		private readonly IVehicleRouteBusiness _vehicleRouteBusiness;
	}
}
