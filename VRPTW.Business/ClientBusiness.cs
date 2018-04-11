using System.Collections.Generic;
using System.Transactions;
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
			using (var transaction = new TransactionScope())
			{
				var client = clientDto.CreateEntity();
				int clientId = _clientRepository.CreateClient(client);
				client.Address.ClientId = clientId;
				_addressRepository.CreateAddres(client.Address);

				transaction.Complete();

				return clientId;
			}
		}

		public void EditClient(ClientDto clientDto)
		{
			using (var transaction = new TransactionScope())
			{
				var client = clientDto.CreateEntity();
				_clientRepository.EditClient(client);
				client.Address.ClientId = client.ClientId;
				_addressRepository.EditAddress(client.Address);
				transaction.Complete();
			}
		}

		public List<ClientDto> GetClients()
		{
			var clients = _clientRepository.GetClients();
			foreach(var client in clients)
			{
				client.Address = _addressRepository.GetAddressByClientId(client.ClientId);
			}
			var clientsDto = clients.CreateDto();
			return clientsDto;
		}

		public ClientBusiness(IClientRepository clientRepository, IAddressRepository addressRepository)
		{
			_clientRepository = clientRepository;
			_addressRepository = addressRepository;
		}

		private readonly IClientRepository _clientRepository;
		private readonly IAddressRepository _addressRepository;
	}
}
