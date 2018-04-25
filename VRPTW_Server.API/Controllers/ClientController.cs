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
		[Route("client/clientbyname/{clientName}")]
		[ResponseType(typeof(List<ClientDto>))]
		public IHttpActionResult GetClients(string clientName)
		{
			try
			{
				var clientsDto = _clientBusiness.GetClientsByName(clientName);
				return Ok(clientsDto);
			}
			catch(Exception e)
			{
				return InternalServerError(e);
			}
		}

		[HttpGet]
		[Route("client/{clientId}")]
		[ResponseType(typeof(ClientDto))]
		public IHttpActionResult GetClientById(int clientId)
		{
			try
			{
				var client = _clientBusiness.GetClientById(clientId);
				return Ok(client);
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
