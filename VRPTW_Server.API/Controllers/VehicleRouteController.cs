using System;
using System.Collections.Generic;
using System.Globalization;
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
		public IHttpActionResult GetVehicleRoutes(string desiredDateInitial, string desiredDateFinal, int? productType = null)
		{
			try
			{
				var vehicleRoutes = _vehicleRouteBusiness.GetVehicleRoutes(new VehicleRouteFilterDto()
				{
					desiredDateInitial = DateTime.Parse(desiredDateInitial),
					desiredDateFinal = DateTime.Parse(desiredDateFinal).AddHours(23).AddMinutes(59).AddSeconds(59),
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
