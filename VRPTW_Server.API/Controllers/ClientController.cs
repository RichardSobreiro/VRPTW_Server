using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using VRPTW.Domain.Dto;
using VRPTW.Domain.Interface.Business;

namespace VRPTW_Server.API.Controllers
{
	public class ClientController : BaseController
    {
		[HttpGet]
		[Route("client/clients")]
		[ResponseType(typeof(List<ClientDto>))]
		public IHttpActionResult GetClients()
		{
			try
			{
				var clientsDto = _clientBusiness.GetClients();
				return Ok(clientsDto);
			}
			catch(Exception e)
			{
				return InternalServerError(e);
			}
		}

		[HttpPost]
		[Route("client/client")]
		[ResponseType(typeof(int))]
		public IHttpActionResult CreateClient([FromBody] ClientDto clientDto)
		{
			try
			{
				var clientId = _clientBusiness.CreateClient(clientDto);
				return Ok(clientId);
			}
			catch(Exception e)
			{
				return InternalServerError(e);
			}
		}

		[HttpPut]
		[Route("client/client")]
		[ResponseType(typeof(void))]
		public IHttpActionResult EditClient([FromBody] ClientDto clientDto)
		{
			try
			{
				_clientBusiness.EditClient(clientDto);
				return Ok();
			}
			catch(Exception e)
			{
				return InternalServerError(e);
			}
		}

		public ClientController(IClientBusiness clientBusiness)
		{
			_clientBusiness = clientBusiness;
		}

		private readonly IClientBusiness _clientBusiness;
    }
}
