using System.Collections.Generic;
using System.Transactions;
using VRPTW.Business.Mapper;
using VRPTW.Domain.Dto;
using VRPTW.Domain.Entity;
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

		public List<ClientDto> GetClientsByName(string clientName)
		{
			var clients = _clientRepository.GetClientsByName(clientName);
			FillClientsAddress(clients);
			var clientsDto = clients.CreateDto();
			return clientsDto;
		}

		public ClientDto GetClientById(int clientId)
		{
			var client = _clientRepository.GetClientById(clientId);
			FillClientAddress(client);
			var clientDto = client.CreateDto();
			return clientDto;
		}

		public ClientBusiness(IClientRepository clientRepository, IAddressRepository addressRepository)
		{
			_clientRepository = clientRepository;
			_addressRepository = addressRepository;
		}

		private void FillClientsAddress(List<Client> clients)
		{
			foreach (var client in clients)
			{
				FillClientAddress(client);
			}
		}

		private void FillClientAddress(Client client)
		{
			client.Address = _addressRepository.GetAddressByClientId(client.ClientId);
		}

		private readonly IClientRepository _clientRepository;
		private readonly IAddressRepository _addressRepository;
	}
}
