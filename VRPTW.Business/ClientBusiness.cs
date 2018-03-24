using System.Collections.Generic;
using VRPTW.Business.Mapper;
using VRPTW.Domain.Dto;
using VRPTW.Domain.Interface.Business;
using VRPTW.Domain.Interface.Repository;

namespace VRPTW.Business
{
	public class ClientBusiness : IClientBusiness
	{
		public int CreateClient(ClientDto clientDto)
		{
			var client = clientDto.CreateEntity();
			int clientId = _clientRepository.CreateClient(client);
			return clientId;
		}

		public void EditClient(ClientDto clientDto)
		{
			var client = clientDto.CreateEntity();
			_clientRepository.EditClient(client);
		}

		public List<ClientDto> GetClients()
		{
			var clients = _clientRepository.GetClients();
			var clientsDto = clients.CreateDto();
			return clientsDto;
		}

		public ClientBusiness(IClientRepository clientRepository)
		{
			_clientRepository = clientRepository;
		}

		private readonly IClientRepository _clientRepository;
	}
}
