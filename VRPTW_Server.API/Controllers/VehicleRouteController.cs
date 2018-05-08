using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using VRPTW.Domain.Dto;
using VRPTW.Domain.Interface.Repository;

namespace VRPTW_Server.API.Controllers
{
	[RoutePrefix("vehicleroute")]
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

		[HttpGet]
		[Route("vehicleroutes")]
		[ResponseType(typeof(List<VehicleRouteDto>))]
		public IHttpActionResult GetVehicleRoutes(DateTime? desiredDateInitial = null, DateTime? desiredDateFinal = null, 
			int? productType = null)
		{
			try
			{
				var vehicleRoutes = _vehicleRouteBusiness.GetVehicleRoutes(new VehicleRouteFilterDto()
				{
					desiredDateInitial = desiredDateInitial,
					desiredDateFinal = desiredDateFinal,
					productType = productType
				});
				return Ok(vehicleRoutes);
			}
			catch(Exception e)
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
