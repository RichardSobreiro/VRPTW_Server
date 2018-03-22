using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;
using System.Web.Http.Description;
using VRPTW.Domain.Dto;
using VRPTW.Domain.Interface.Business;

namespace VRPTW_Server.API.Controllers
{
	public class DeliveryController : BaseController
	{
		[HttpPost]
		[Route("fractioneddelivery")]
		[ResponseType(typeof(int))]
		public IHttpActionResult CreateFractionedDelivery([FromBody] JObject _fractionedDeliveryDto)
		{
			try
			{
				var fractionedDeliveryDto = JsonConvert.DeserializeObject<DeliveryDto>(JsonConvert.SerializeObject(_fractionedDeliveryDto));
				return Ok(_createDeliveryBusiness.CreateFractionedDelivery(fractionedDeliveryDto));
			}
			catch(Exception e)
			{
				return(InternalServerError(e));
			}
		}

		public DeliveryController(ICreateDeliveryBusiness createDeliveryBusiness)
		{
			_createDeliveryBusiness = createDeliveryBusiness;
		}

		private readonly ICreateDeliveryBusiness _createDeliveryBusiness;
	}
}
